using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iHentai.Common;
using iHentai.Data;
using iHentai.Extensions.Common;
using iHentai.Extensions.Models;
using iHentai.Services;
using Newtonsoft.Json;

namespace iHentai.Extensions
{
    public class ExtensionManager
    {
        public ExtensionManager()
        {
            Init();
        }

        private readonly Dictionary<string, ScriptEngine> _cacheEngines = new Dictionary<string, ScriptEngine>();
        private readonly Dictionary<ExtensionManifest, IMangaApi> _cacheApis = new Dictionary<ExtensionManifest, IMangaApi>();

        public Dictionary<string, ExtensionManifest> Extensions { get; } =
            new Dictionary<string, ExtensionManifest>();

        public string ExtensionPath
        {
            get => SettingsDb.Instance.Get("extension_path", null) ?? Path.Combine(Environment.CurrentDirectory, "Extensions");
            set => SettingsDb.Instance.Set("extension_path", value);
        }

        private void Init()
        {
            if (Directory.Exists(ExtensionPath))
            {
                foreach (var directory in Directory.GetDirectories(ExtensionPath))
                {
                    var manifest = Path.Combine(directory, "manifest.json");
                    if (!File.Exists(manifest))
                    {
                        continue;
                    }

                    var data = JsonConvert.DeserializeObject<ExtensionManifest>(
                        File.ReadAllText(manifest));
                    Extensions.Add(
                        PathHelper.MakeRelativePath(Environment.CurrentDirectory + Path.DirectorySeparatorChar,
                            directory), data);
                }
            }
        }

        public async Task<ScriptEngine> GetScriptEngineAsync(string path)
        {
            if (!Extensions.ContainsKey(path))
            {
                throw new ArgumentException(nameof(path));
            }

            if (_cacheEngines.ContainsKey(path))
            {
                return _cacheEngines[path];
            }

            var manifest = Extensions[path];
            var engine = new ScriptEngine(path, manifest);
            await engine.Init(path);
            _cacheEngines.Add(path, engine);
            return engine;
        }

        public async Task<IMangaApi?> GetApi(ExtensionManifest manifest)
        {
            if (_cacheApis.ContainsKey(manifest))
            {
                return _cacheApis[manifest];
            }
            var item = Extensions.FirstOrDefault(it => it.Value == manifest);
            if (!string.IsNullOrEmpty(item.Key))
            {
                var api = new ScriptApi(await GetScriptEngineAsync(item.Key), item.Key, manifest);
                HentaiHttpHandler.RegisterHandler(api);
                _cacheApis.Add(manifest, api);
                return api;
            }

            return null;
        }
    }
}