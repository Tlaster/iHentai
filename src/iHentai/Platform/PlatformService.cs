using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace iHentai.Platform
{
    internal class PlatformService : IPlatformService
    {
        public string LocalPath => ApplicationData.Current.LocalFolder.Path;

        public async Task<IFolderItem?> GetFolder(string token)
        {
            var folder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(token);
            if (folder == null)
            {
                return null;
            }

            return new PathFolderItem(folder.Path, token, folder.DateCreated.DateTime);
        }

        public async Task<IFolderItem?> GetFolderFromPath(string path, string token)
        {
            var folder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(token);
            if (folder == null)
            {
                return null;
            }

            return new PathFolderItem(path, token, folder.DateCreated.DateTime);
        }

        public async Task<IFolderItem?> GetFolderFromPath(string path)
        {
            try
            {
                var folder = await StorageFolder.GetFolderFromPathAsync(path);
                return new FolderItem(folder, "");
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}