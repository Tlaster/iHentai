using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml.Data;
using iHentai.Common.Extensions;
using iHentai.Data.Models;
using iHentai.Platform;
using iHentai.Services;
using Microsoft.Toolkit.Uwp.UI;
using PropertyChanged;

namespace iHentai.ViewModels.Local
{
    internal class LocalLibraryViewModel : ViewModelBase
    {
        public bool IsLoading { get; private set; }

        public IEnumerable<LocalGalleryModel> Source => LocalLibraryManager.Instance.LocalGallery;
        public AdvancedCollectionView SourceView { get; } = new AdvancedCollectionView(LocalLibraryManager.Instance.LocalGallery);

        public LocalLibraryViewModel()
        {
        }

        public async void SelectFolder()
        {
            var picker = new FolderPicker();
            picker.InitWindow();
            picker.FileTypeFilter.Add("*");
            var result = await picker.PickSingleFolderAsync();
            if (result != null)
            {
                var token = StorageApplicationPermissions.FutureAccessList.Add(result);
                IsLoading = true;
                var folder = await HentaiApp.Instance.Resolve<IPlatformService>().GetFolder(token);
                await LocalLibraryManager.Instance.AddFolder(folder);
                IsLoading = false;
            }
        }

        public async void Refresh()
        {
            IsLoading = true;
            await LocalLibraryManager.Instance.Refresh();
            IsLoading = false;
        }

        public void Filter(string queryText)
        {
            SourceView.Filter = o => (o as LocalGalleryModel).Name.Contains(queryText, StringComparison.InvariantCultureIgnoreCase);
        }

        public void ClearFilter()
        {
            SourceView.Filter = o => true;
        }
    }

}
