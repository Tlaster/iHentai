using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace iHentai.Platform
{
    class PlatformService : IPlatformService
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
            var folder = await StorageFolder.GetFolderFromPathAsync(path);
            return new FolderItem(folder, "");
        }
    }
}
