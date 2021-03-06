﻿using System;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;
using iHentai.Common.Extensions;
using iHentai.Data.Models;
using iHentai.ViewModels.Archive;
using iHentai.ViewModels.Local;
using Microsoft.Toolkit.Uwp.UI.Extensions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Pages.Local
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LocalLibraryPage : Page
    {
        public LocalLibraryPage()
        {
            InitializeComponent();
        }

        internal LocalLibraryViewModel ViewModel { get; } = new LocalLibraryViewModel();

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is LocalGalleryModel item)
            {
                this.FindAscendant<RootView>().ContentFrame
                    .Navigate(typeof(ReadingPage), new LocalReadingViewModel(item));
            }
        }

        private void AutoSuggestBox_OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ViewModel.Filter(args.QueryText);
        }

        private void AutoSuggestBox_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (sender.Text.Length == 0)
            {
                ViewModel.ClearFilter();
            }
        }

        private async void SelectFile()
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".epub");
            picker.FileTypeFilter.Add(".cbz");
            picker.FileTypeFilter.Add(".cbr");
            picker.FileTypeFilter.Add(".cb7");
            picker.FileTypeFilter.Add(".cbt");
            picker.FileTypeFilter.Add(".zip");
            picker.FileTypeFilter.Add(".rar");
            picker.FileTypeFilter.Add(".7z");
            picker.FileTypeFilter.Add(".pdf");
            var result = await picker.PickSingleFileAsync();
            if (result != null)
            {
                this.FindAscendant<RootView>().ContentFrame.Navigate(typeof(ReadingPage), result.GetFileReadingViewModel());
            }
        }
    }
}