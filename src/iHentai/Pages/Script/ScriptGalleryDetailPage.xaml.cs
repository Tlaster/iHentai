using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using iHentai.Services.Models.Script;
using iHentai.ViewModels.Local;
using iHentai.ViewModels.Script;
using Microsoft.Toolkit.Uwp.UI.Extensions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Pages.Script
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ScriptGalleryDetailPage : Page
    {
        ScriptGalleryDetailViewModel ViewModel { get; set; }

        public ScriptGalleryDetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is ScriptGalleryDetailViewModel viewModel)
            {
                ViewModel = viewModel;
            }
        }

        private void GoBackClicked(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void ReadClicked(object sender, RoutedEventArgs e)
        {
            
        }

        private async void ContentPresenter_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.Tag is ScriptGalleryChapter chapter && ViewModel.Detail != null)
            {
                if (await ViewModel.Api.CheckCanOpenChapter(chapter))
                {
                    this.FindAscendant<RootView>().ContentFrame.Navigate(typeof(ReadingPage), new ScriptReadingViewModel(ViewModel.Api, ViewModel.Detail, chapter));
                }
            }
        }
    }
}
