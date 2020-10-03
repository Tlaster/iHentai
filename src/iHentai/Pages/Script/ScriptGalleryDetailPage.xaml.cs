using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using iHentai.Services.Models.Script;
using iHentai.ViewModels.Script;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Extensions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Pages.Script
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ScriptGalleryDetailPage : Page
    {
        public ScriptGalleryDetailPage()
        {
            InitializeComponent();
        }

        private ScriptGalleryDetailViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is ScriptGalleryDetailViewModel viewModel)
            {
                ViewModel = viewModel;
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            Frame.SetListDataItemForNextConnectedAnimation(ViewModel.Gallery);
        }

        private async void ReadClicked(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Detail != null)
            {
                if (ViewModel.Detail.Chapters != null && ViewModel.Detail.Chapters.Any())
                {
                    var firstChapter = ViewModel.Detail.Chapters.FirstOrDefault();
                    if (firstChapter != null && await ViewModel.CheckCanOpenChapter(firstChapter))
                    {
                        this.FindAscendant<RootView>().ContentFrame.Navigate(typeof(ReadingPage),
                            new ScriptChapterReadingViewModel(ViewModel.Api, ViewModel.Detail, firstChapter));
                    }
                } 
                else if (ViewModel.Detail.Images != null && ViewModel.Detail.Images.Any())
                {
                    this.FindAscendant<RootView>().ContentFrame.Navigate(typeof(ReadingPage),
                        new ScriptGalleryReadingViewModel(ViewModel.Api, ViewModel.Detail));
                }
            }
        }

        private async void ContentPresenter_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.Tag is ScriptGalleryChapter chapter &&
                ViewModel.Detail != null)
            {
                if (await ViewModel.CheckCanOpenChapter(chapter))
                {
                    this.FindAscendant<RootView>().ContentFrame.Navigate(typeof(ReadingPage),
                        new ScriptChapterReadingViewModel(ViewModel.Api, ViewModel.Detail, chapter));
                }
            }
        }

        private void TagClicked(object sender, RoutedEventArgs e)
        {

        }

        private void ShowAllImagesClicked(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Detail?.Images != null)
            {
                Frame.Navigate(typeof(ScriptGalleryImagesPage), new ScriptGalleryImagesViewModel(ViewModel.Detail.Images));
            }
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            ViewModel.Refresh();
        }
    }
}