using System;
using System.Collections.Generic;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using iHentai.Common;
using iHentai.Common.Extensions;
using iHentai.Data;
using iHentai.Data.Models;
using iHentai.Extensions.Models;
using iHentai.Services;

namespace iHentai.ViewModels
{
    class SettingsViewModel : ViewModelBase
    {
        public bool IsLoading { get; private set; }
        public IEnumerable<LocalLibraryModel> LocalLibrary => LocalLibraryManager.Instance.LocalLibrary;
        public string ExtensionFolder => SettingsManager.Instance.ExtensionFolder.Path ?? "";
        

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

        public async void SetExtensionFolder()
        {
            var picker = new FolderPicker();
            picker.InitWindow();
            picker.FileTypeFilter.Add("*");
            var result = await picker.PickSingleFolderAsync();
            if (result != null)
            {
                if (StorageApplicationPermissions.FutureAccessList.CheckAccess(result))
                {
                    return;
                }
                var token = StorageApplicationPermissions.FutureAccessList.Add(result);
                var current = SettingsManager.Instance.ExtensionFolder;
                if (current.Token != null)
                {
                    StorageApplicationPermissions.FutureAccessList.Remove(current.Token);
                }

                SettingsManager.Instance.ExtensionFolder = new ExtensionFolderData {Path = result.Path, Token = token};
                OnPropertyChanged(nameof(ExtensionFolder));
            }
        }
    }
}