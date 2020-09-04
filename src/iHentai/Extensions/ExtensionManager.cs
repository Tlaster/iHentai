using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using iHentai.Common;
using iHentai.Extensions.Common;
using iHentai.Extensions.Models;
using iHentai.Platform;
using iHentai.Services;
using Newtonsoft.Json;

namespace iHentai.Extensions
{
    public interface IExtensionManager
    {
        ObservableCollection<ExtensionManifest> Extensions { get; }
        void Reload();
        Task<IMangaApi?> GetApi(ExtensionManifest manifest);
    }

    public class ExtensionManager : IExtensionManager
    {
        private readonly Dictionary<ExtensionManifest, IMangaApi> _cacheApis =
            new Dictionary<ExtensionManifest, IMangaApi>();

        private readonly Dictionary<string, ScriptEngine> _cacheEngines = new Dictionary<string, ScriptEngine>();

        private readonly Dictionary<string, ExtensionManifest> _extensions =
            new Dictionary<string, ExtensionManifest>();

        public ExtensionManager()
        {
            Init();
        }

        public ObservableCollection<ExtensionManifest> Extensions { get; } =
            new ObservableCollection<ExtensionManifest>();

        public void Reload()
        {
            _cacheApis.Clear();
            _cacheApis.Clear();
            _extensions.Clear();
            Extensions.Clear();
            Init();
        }

        public async Task<IMangaApi?> GetApi(ExtensionManifest manifest)
        {
            if (_cacheApis.ContainsKey(manifest))
            {
                return _cacheApis[manifest];
            }

            var item = _extensions.FirstOrDefault(it => it.Value == manifest);
            if (!string.IsNullOrEmpty(item.Key))
            {
                var api = new ScriptApi(await GetScriptEngineAsync(item.Key), item.Key, manifest);
                HentaiHttpHandler.RegisterHandler(api);
                _cacheApis.Add(manifest, api);
                return api;
            }

            return null;
        }

        private async Task Init()
        {
            var folderData = SettingsManager.Instance.ExtensionFolder;
            if (folderData.Path != null)
            {
                var folder = folderData.Token == null
                    ? await this.Resolve<IPlatformService>().GetFolderFromPath(folderData.Path)
                    : await this.Resolve<IPlatformService>().GetFolderFromPath(folderData.Path, folderData.Token);
                if (folder != null)
                {
                    var folders = await folder.GetFolders();
                    foreach (var directory in folders)
                    {
                        var files = await directory.GetFiles();
                        var manifest = files.FirstOrDefault(it => it.Name == "manifest.json");
                        if (manifest == null)
                        {
                            continue;
                        }
                        
                        var data = JsonConvert.DeserializeObject<ExtensionManifest>(
                            await manifest.ReadAllTextAsync());
                        _extensions.Add(directory.Path, data);
                        Extensions.Add(data);
                    }
                }
            }
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
    }
}