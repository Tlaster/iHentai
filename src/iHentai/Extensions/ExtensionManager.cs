using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using iHentai.Common;
using iHentai.Extensions.Models;
using iHentai.Services;

namespace iHentai.Extensions
{
    public abstract class ExtensionManager : IExtensionManager
    {
        private readonly Dictionary<ExtensionManifest, ScriptApi> _cacheApis =
            new Dictionary<ExtensionManifest, ScriptApi>();

        private readonly Dictionary<string, ScriptEngine> _cacheEngines = new Dictionary<string, ScriptEngine>();

        private readonly Dictionary<string, ExtensionManifest> _extensions =
            new Dictionary<string, ExtensionManifest>();

        public ObservableCollection<ExtensionManifest> Extensions { get; } = new ObservableCollection<ExtensionManifest>();
        
        public async Task<ScriptApi?> GetApi(ExtensionManifest manifest)
        {
            if (_cacheApis.ContainsKey(manifest))
            {
                return _cacheApis[manifest];
            }

            var (key, _) = _extensions.FirstOrDefault(it => it.Value == manifest);
            if (!string.IsNullOrEmpty(key))
            {
                var api = new ScriptApi(await GetScriptEngineAsync(key), key, manifest);
                HentaiHttpHandler.Instance.RegisterHandler(api);
                _cacheApis.Add(manifest, api);
                return api;
            }

            return null;
        }

        private async Task<ScriptEngine> GetScriptEngineAsync(string path)
        {
            if (!_extensions.ContainsKey(path))
            {
                throw new ArgumentException(nameof(path));
            }

            if (_cacheEngines.ContainsKey(path))
            {
                return _cacheEngines[path];
            }

            var manifest = _extensions[path];
            var engine = new ScriptEngine(path, manifest);
            await engine.Init(path);
            _cacheEngines.Add(path, engine);
            return engine;
        }

        public async Task Reload()
        {
            _cacheApis.Clear();
            _extensions.Clear();
            Extensions.Clear();
            Init();
        }

        public async Task<ScriptApi?> GetApiById(string extensionId)
        {
            var manifest = _extensions.GetValueOrDefault(extensionId);
            return manifest == null ? null : await GetApi(manifest);
        }

        public virtual async Task Init()
        {
            var extension = await GetExtensions();
            foreach (var (key, value) in extension)
            {
                AddOrUpgradeExtension(key, value);
            }
        }

        protected void AddOrUpgradeExtension(string path, ExtensionManifest manifest)
        {
            if (_extensions.ContainsKey(path))
            {
                var current = _extensions[path];
                _extensions[path] = manifest;
                Extensions.Remove(current);
                Extensions.Add(manifest);
                _cacheEngines.Remove(path);
                _cacheApis.Remove(current);
            }
            else
            {
                _extensions.Add(path, manifest);
                Extensions.Add(manifest);
            }
        }

        protected void RemoveExtension(string path)
        {
            var current = _extensions[path];
            _extensions.Remove(path);
            Extensions.Remove(current);
            _cacheEngines.Remove(path);
            _cacheApis.Remove(current);
        }

        protected abstract Task<Dictionary<string, ExtensionManifest>> GetExtensions();

    }
}