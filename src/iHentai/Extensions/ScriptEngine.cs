using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Esprima;
using Esprima.Ast;
using iHentai.Common;
using iHentai.Extensions.Models;
using iHentai.Extensions.Runtime;
using iHentai.Extensions.Runtime.Html;
using Jint;
using Jint.Native;
using Jint.Native.Json;
using Jint.Runtime;
using LZStringCSharp;
using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json;

namespace iHentai.Extensions
{
    public class ScriptEngine 
    {
        private readonly string _extensionId;
        private readonly ExtensionManifest _manifest;
        private readonly Fetch _fetch;
        private readonly LocalStorage _storage;
        private readonly Log _log;
        private readonly ObjectPool<Engine> _enginePool;
        private Script _script;
        private List<string> _properties;

        public ScriptEngine(string extensionId, ExtensionManifest manifest)
        {
            _extensionId = extensionId;
            _manifest = manifest;
            _fetch = new Fetch(manifest, HentaiHttpHandler.Instance);
            _storage = new LocalStorage(_extensionId, HentaiApp.Instance.Resolve<IExtensionStorage>());
            _log = new Log(_extensionId);
            _enginePool = new ObjectPool<Engine>(GetEngine);
        }


        public async Task Init(string path)
        {
            if (_manifest.Entry == null)
            {
                throw new ArgumentNullException("Manifest should have entry");
            }

            var filePath = Path.Combine(path, _manifest.Entry);
            var file = await StorageFile.GetFileFromPathAsync(filePath);
            var entryFile = await FileIO.ReadTextAsync(file);
            var parser = new JavaScriptParser(entryFile);
            _script = parser.ParseScript();
            var engine = _enginePool.Get();
            _properties = engine.Global.GetOwnPropertyKeys().Select(it => it.ToString()).ToList();
            _enginePool.Return(engine);
        }

        private Engine GetEngine()
        {
            var engine = new Engine();
            engine.SetValue("localStorage", _storage);
            engine.SetValue("console", _log);
            engine.SetValue("fetch", new Func<string, JsValue, FetchResponse>((s, value) =>
            {
                FetchInit? init = null;
                if (value != null)
                {
                    init = JsonConvert.DeserializeObject<FetchInit>(engine.Json.Stringify(engine.Global, new[] {value})
                        .ToString());
                }

                return _fetch.fetch(s, init);
            }));
            engine.SetValue("parseHtml", new Func<string, HtmlElement>(HtmlElement.Parse));
            engine.SetValue("unpack", new Func<string, string?>(UnPacker.Unpack));
            engine.SetValue("decodeLzStringFromBase64", new Func<string, string>(LZString.DecompressFromBase64));
            engine.Execute(_script);
            return engine;
        }


        public Task<T> InvokeFunctionAsync<T>(string name, params object[] arguments)
        {
            return Task.Run(() => InvokeFunction<T>(name, arguments));
        }

        public T InvokeFunction<T>(string name, params object[] arguments)
        {
            var engine = _enginePool.Get();
            try
            {
                var result = engine.Invoke(name, arguments);
                var json = engine.Json.Stringify(engine.Global, new []{result}).ToString();
                if (json is T tvalue)
                {
                    return tvalue;
                }

                return JsonConvert.DeserializeObject<T>(json);
            }
            finally
            {
                _enginePool.Return(engine);
            }
        }

        public bool HasMember(string name)
        {
            return _properties.Contains(name);
        }
    }
}