using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Esprima;
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
        public Engine Engine { get; } = new Engine();
        private readonly Fetch _fetch;

        public ScriptEngine(string extensionId, ExtensionManifest manifest)
        {
            _extensionId = extensionId;
            _manifest = manifest;
            _fetch = new Fetch(manifest, HentaiHttpHandler.Instance);
        }

        public JsonInstance JSON => Engine.Json;

        public async Task Init(string path)
        {
            if (_manifest.Entry == null)
            {
                throw new ArgumentNullException("Manifest should have entry");
            }

            var filePath = Path.Combine(path, _manifest.Entry);
            var file = await StorageFile.GetFileFromPathAsync(filePath);
            var entryFile = await FileIO.ReadTextAsync(file);

            Engine.SetValue("localStorage",
                new LocalStorage(_extensionId, HentaiApp.Instance.Resolve<IExtensionStorage>()));
            Engine.SetValue("console", new Log(_extensionId));
            Engine.SetValue("fetch", new Func<string, JsValue, FetchResponse>((s, value) =>
            {
                FetchInit? init = null;
                if (value != null)
                {
                    init = JsonConvert.DeserializeObject<FetchInit>(Engine.Json.Stringify(Engine.Global, new[] {value})
                        .ToString());
                }

                return _fetch.fetch(s, init);
            }));
            Engine.SetValue("parseHtml", new Func<string, HtmlElement>(HtmlElement.Parse));
            Engine.SetValue("unpack", new Func<string, string?>(UnPacker.Unpack));
            Engine.SetValue("decodeLzStringFromBase64", new Func<string, string>(LZString.DecompressFromBase64));
            Engine.Execute(entryFile);
        }


        public async Task<T> InvokeFunctionAsync<T>(string name, params object[] arguments)
        {
            return await DispatcherHelper.ExecuteOnUIThreadAsync(async () => await Task.Run(() => InvokeFunction<T>(name, arguments)));
        }

        public T InvokeFunction<T>(string name, params object[] arguments)
        {
            var result = Engine.Invoke(name, arguments);
            var json = JSON.Stringify(Engine.Global, new []{result}).ToString();
            if (json is T tvalue)
            {
                return tvalue;
            }

            return JsonConvert.DeserializeObject<T>(json);
        }

        public bool HasMember(string name)
        {
            return Engine.GetValue(name).Type != Types.Undefined;
        }
    }
}