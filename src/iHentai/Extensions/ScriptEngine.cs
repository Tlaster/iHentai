using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using iHentai.Extensions.Models;
using iHentai.Extensions.Runtime;
using iHentai.Extensions.Runtime.Html;
using NiL.JS;
using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using NiL.JS.Extensions;

namespace iHentai.Extensions
{
    public class ScriptEngine : IModuleResolver
    {
        private readonly string _extensionId;
        private readonly ExtensionManifest _manifest;

        private readonly StringMap<Module> _modulesCache = new StringMap<Module>();
        private Fetch _fetch;
        private Module _module;

        public ScriptEngine(string extensionId, ExtensionManifest manifest)
        {
            _extensionId = extensionId;
            _manifest = manifest;
        }

        //public JSValue ExtensionModules { get; private set; }


        public bool TryGetModule(ModuleRequest moduleRequest, out Module result)
        {
            var cacheKey = GetCacheKey(moduleRequest);

            return _modulesCache.TryGetValue(cacheKey, out result);
        }

        public async Task Init(string path)
        {
            if (_manifest.Entry == null)
            {
                throw new ArgumentNullException("Manifest should have entry");
            }

            var entryFile =
                File.ReadAllText(Path.Combine(Environment.CurrentDirectory, path, _manifest.Entry));
            _module = new Module(_manifest.Entry, entryFile);
            if (_manifest.Modules != null && _manifest.Modules.Any())
            {
                foreach (var item in _manifest.Modules)
                {
                    var moduleFile =
                        File.ReadAllText(Path.Combine(Environment.CurrentDirectory, path, item));
                    _modulesCache.Add(item.TrimStart('.'), new Module(moduleFile));
                }
            }

            _module.ModuleResolversChain.Add(this);
            _fetch = new Fetch(_manifest, HentaiApp.Instance.Resolve<HttpMessageHandler>());
            _module.Context.DefineVariable("localStorage")
                .Assign(JSValue.Marshal(new LocalStorage(_extensionId, HentaiApp.Instance.Resolve<IExtensionStorage>())));
            _module.Context.DefineVariable("debug").Assign(JSValue.Marshal(new Log(_extensionId)));
            _module.Context.DefineVariable("fetch")
                .Assign(JSValue.Marshal(new Func<string, JSObject?, Task<JSValue>>(_fetch.fetch)));
            _module.Context.DefineVariable("parseHtml").Assign(JSValue.Marshal(new Func<string, JSValue>(s => JSValue.Marshal(HtmlElement.Parse(s)))));
            //_module.Context.DefineVariable("registerExtension")
            //    .Assign(JSValue.Marshal(new Func<JSValue, bool>(value =>
            //    {
            //        ExtensionModules = value;
            //        return true;
            //    })));
            _module.Run();
        }

        public async Task<T> InvokeFunctionAsync<T>(string name, Arguments arguments)
        {
            var result = _module.Context.GetVariable(name).As<Function>().Call(arguments);
            var promise = result.As<Promise>();
            if (promise != null)
            {
                var promiseResult = await promise.Task;
                return promiseResult.As<T>();
            }

            return result.As<T>();
        }

        public T InvokeFunction<T>(string name, Arguments arguments)
        {
            return _module.Context.GetVariable(name).As<Function>().Call(arguments).As<T>();
        }

        public bool HasMember(string name)
        {
            var member = _module.Context.GetVariable(name);
            return member != null && member.Exists;
        }


        public virtual string GetCacheKey(ModuleRequest moduleRequest)
        {
            return moduleRequest.AbsolutePath;
        }

        public void RemoveFromCache(string key)
        {
            _modulesCache.Remove(key);
        }

        public void ClearCache()
        {
            _modulesCache.Clear();
        }
    }
}