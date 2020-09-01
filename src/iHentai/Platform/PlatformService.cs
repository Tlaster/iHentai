using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace iHentai.Platform
{
    class PlatformService : IPlatformService
    {
        public string LocalPath => ApplicationData.Current.LocalFolder.Path;

        public async Task<IFolderItem> GetFolder(string token)
        {
            var folder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(token);
            return new FolderItem(folder, token);
        }

        public async Task<IFolderItem> GetFolderFromPath(string path, string token)
        {
            return new FolderItem(await StorageFolder.GetFolderFromPathAsync(path), token);
        }
    }
}
