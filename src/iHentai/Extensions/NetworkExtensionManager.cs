using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Storage;
using iHentai.Extensions.Models;
using Microsoft.Toolkit.Uwp.UI;
using Newtonsoft.Json;

namespace iHentai.Extensions
{
    public class NetworkExtensionManager : ExtensionManager, INetworkExtensionManager
    {
        private const string EXTENSION_CHANNEL = "master";

        private readonly string EXTENSION_REMOTE =
            $"https://raw.githubusercontent.com/HentaiFoundation/iHentai-Extensions/{EXTENSION_CHANNEL}/";

        public override async Task Init()
        {
            await base.Init();
            NetworkExtension.Clear();
            try
            {    
                var remote = await GetRemoteExtensionList();
                foreach (var item in remote)
                {
                    NetworkExtension.Add(item);
                }
                foreach (var manifest in Extensions)
                {
                    var update = remote.FirstOrDefault(it => it.Name == manifest.Name && it.Version != manifest.Version);
                    if (update != null)
                    {
                        await InstallExtension(update);
                    }
                }
            }
            catch
            {
                // ignored
            }
        }

        public ObservableCollection<NetworkExtensionModel> NetworkExtension { get; } = new ObservableCollection<NetworkExtensionModel>();

        public async Task<List<NetworkExtensionModel>> GetRemoteExtensionList()
        {
            using var client = new HttpClient();
            var data = await client.GetStringAsync(Path.Combine(EXTENSION_REMOTE, "manifest.json"));
            return JsonConvert.DeserializeObject<List<NetworkExtensionModel>>(data);
        }

        public async Task<ExtensionManifest> InstallExtension(NetworkExtensionModel model)
        {
            using var client = new HttpClient();
            var json = await client.GetStringAsync(Path.Combine(EXTENSION_REMOTE, model.Src, "manifest.json"));
            var manifest = JsonConvert.DeserializeObject<ExtensionManifest>(json);
            var cache = await FileCache.Instance.GetFromCacheAsync(new Uri(Path.Combine(EXTENSION_REMOTE, model.Src,
                manifest.Entry)));
            var rootFolder = await GetExtensionFolder();
            var extensionFolder = await rootFolder.CreateFolderAsync(manifest.Name, CreationCollisionOption.OpenIfExists);
            await cache.CopyAsync(extensionFolder, manifest.Entry, NameCollisionOption.ReplaceExisting);
            var manifestFile = await extensionFolder.CreateFileAsync("manifest.json", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(manifestFile, json);
            AddOrUpgradeExtension(extensionFolder.Path, manifest);
            return manifest;
        }

        public async Task UnInstallExtension(ExtensionManifest model)
        {
            var rootFolder = await GetExtensionFolder();
            var extensionFolder = await rootFolder.GetFolderAsync(model.Name);
            await extensionFolder.DeleteAsync(StorageDeleteOption.PermanentDelete);
            RemoveExtension(extensionFolder.Path);
        }

        private async Task<StorageFolder> GetExtensionFolder()
        {
            return await ApplicationData.Current.LocalCacheFolder.CreateFolderAsync("networkExtension",
                CreationCollisionOption.OpenIfExists);
        }

        protected override async Task<Dictionary<string, ExtensionManifest>> GetExtensions()
        {
            var folder = await GetExtensionFolder();
            var folders = await folder.GetFoldersAsync();
            var extensions = new Dictionary<string, ExtensionManifest>();
            foreach (var directory in folders)
            {
                var files = await directory.GetFilesAsync();
                var manifest = files.FirstOrDefault(it => it.Name == "manifest.json");
                if (manifest == null)
                {
                    continue;
                }

                var data = JsonConvert.DeserializeObject<ExtensionManifest>(
                    await FileIO.ReadTextAsync(manifest));
                extensions.Add(directory.Path, data);
            }

            return extensions;
        }
    }
}