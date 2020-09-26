using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using iHentai.Common;
using iHentai.Extensions.Models;
using iHentai.Platform;
using iHentai.Services;
using Newtonsoft.Json;

namespace iHentai.Extensions
{
    public class LocalExtensionManager : ExtensionManager
    {
        protected override async Task<Dictionary<string, ExtensionManifest>> GetExtensions()
        {
            var extensions = new Dictionary<string, ExtensionManifest>();
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
                        extensions.Add(directory.Path, data);
                    }
                }
            }

            return extensions;
        }
    }
}