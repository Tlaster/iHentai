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

        public AdvancedCollectionView SourceView { get; } = new AdvancedCollectionView(LocalLibraryManager.Instance.LocalGallery);
        public LocalFilterViewModel FilterViewModel { get; }

        public LocalLibraryViewModel()
        {
            FilterViewModel = new LocalFilterViewModel(SourceView);
        }

        public async void SelectFolder()
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

        public async void Refresh()
        {
            if (IsLoading)
            {
                return;
            }
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


        internal class LocalFilterViewModel
        {
            private readonly AdvancedCollectionView _collection;
            private SortDescription? _sortDescription;

            public LocalFilterViewModel(AdvancedCollectionView collection)
            {
                _collection = collection;

            }

            public SortDescription? SortDescription
            {
                get => _sortDescription;
                set
                {
                    _collection.SortDescriptions.Clear();
                    if (value != null)
                    {
                        _collection.SortDescriptions.Add(value);
                    }
                    _sortDescription = value;
                }
            }

            public bool IsDefault
            {
                get => SortDescription == null;
                set => SortDescription = null;
            }

            public bool IsNewest
            {
                get =>
                    SortDescription != null && SortDescription.PropertyName == nameof(LocalGalleryModel.CreationTime) &&
                    SortDescription.Direction == SortDirection.Descending;
                set => SortDescription = new SortDescription(nameof(LocalGalleryModel.CreationTime), SortDirection.Descending);
            }

            public bool IsOldest
            {
                get =>
                    SortDescription != null && SortDescription.PropertyName == nameof(LocalGalleryModel.CreationTime) &&
                    SortDescription.Direction == SortDirection.Ascending;
                set => SortDescription = new SortDescription(nameof(LocalGalleryModel.CreationTime), SortDirection.Ascending);
            }
        }
    }


}
