using System;
using System.Collections.Generic;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using iHentai.Common;
using iHentai.Common.Extensions;
using iHentai.Data;
using iHentai.Data.Models;
using iHentai.Services;

namespace iHentai.ViewModels
{
    class SettingsViewModel : ViewModelBase
    {
        public bool IsLoading { get; private set; }
        public IEnumerable<LocalLibraryModel> LocalLibrary => LocalLibraryManager.Instance.LocalLibrary;

        public ElementTheme Theme
        {
            get => SettingsManager.Instance.Theme;
            set
            {
                SettingsManager.Instance.Theme = value;
                OnPropertyChanged();
            }
        }

        public async void AddLibraryFolder()
        {
            var picker = new FolderPicker();
            picker.InitWindow();
            picker.FileTypeFilter.Add("*");
            var result = await picker.PickSingleFolderAsync();
            if (result != null)
            {
                IsLoading = true;
                await LocalLibraryManager.Instance.AddFolder(result);
                IsLoading = false;
            }
        }

        public void RemoveLibraryFolder(LocalLibraryModel item)
        {
            LocalLibraryManager.Instance.RemoveFolder(item);
        }
    }
}