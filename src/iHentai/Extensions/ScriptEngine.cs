using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using iHentai.Common;
using iHentai.Extensions.Hosting;
using iHentai.Extensions.Models;
using iHentai.Scripting.Runtime;
using LZStringCSharp;
using Newtonsoft.Json;
using Console = iHentai.Scripting.Runtime.Console;

namespace iHentai.Extensions
{
    public class ScriptEngine 
    {
        private readonly string _extensionId;
        private readonly ExtensionManifest _manifest;
        private readonly ChakraHost _host;

        public ScriptEngine(string extensionId, ExtensionManifest manifest)
        {
            _extensionId = extensionId;
            _manifest = manifest;
            _host = new ChakraHost();
            _host.EnterContext();
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

            _host.DefineProperty("console", new Console());
            _host.DefineProperty("localStorage", new LocalStorage(_extensionId, HentaiApp.Instance.Resolve<IExtensionStorage>()));
            _host.DefineProperty("window", new RootRuntime(HentaiHttpHandler.Instance));

            _host.RunScript(@"
this.parseHtml = window.parseHtml;
this.unpack = window.unpack;
this.decodeLzStringFromBase64 = window.decodeLzStringFromBase64;
this.fetch = window.fetch;
");
            _host.RunScript(entryFile);
            _host.RunScript(@"debugger; window.fetch('https://www.baidu.com', {
        method: 'POST',
        bodyType: 'UrlEncoded'
    });");

            await Task.Delay(10000);
        }


        public async Task<T> InvokeFunctionAsync<T>(string name, params object[] arguments)
        {
            throw new NotImplementedException();
            //var result = _module.Context.GetVariable(name).As<Function>().Call(arguments);
            //if (result.Is<Promise>())
            //{
            //    var task = result.As<Promise>().Task;
            //    var promiseResult = await task;
            //    return promiseResult.As<T>();
            //}

            //return result.As<T>();
        }

        public T InvokeFunction<T>(string name, params object[] arguments)
        {
            throw new NotImplementedException();
            //return _module.Context.GetVariable(name).As<Function>().Call(arguments).As<T>();
        }

        public bool HasMember(string name)
        {
            return _host.GlobalObject.HasProperty(JavaScriptPropertyId.FromString(name));
        }
    }
}