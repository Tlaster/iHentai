using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Popups;
using iHentai.Common.Extensions;
using iHentai.Data.Models;
using iHentai.Platform;
using iHentai.Services;

namespace iHentai.ViewModels.Local
{
    internal class LocalLibraryViewModel : ViewModelBase
    {
        public bool Loading { get; private set; }

        public IEnumerable<LocalGalleryModel> Source => LocalLibraryManager.Instance.LocalGallery;

        public async void SelectFolder()
        {
            var picker = new FolderPicker();
            picker.InitWindow();
            picker.FileTypeFilter.Add("*");
            var result = await picker.PickSingleFolderAsync();
            if (result != null)
            {
                var token = StorageApplicationPermissions.FutureAccessList.Add(result);
                Loading = true;
                var folder = await HentaiApp.Instance.Resolve<IPlatformService>().GetFolder(token);
                await LocalLibraryManager.Instance.AddFolder(folder);
                Loading = false;
            }
        }
    }
}
