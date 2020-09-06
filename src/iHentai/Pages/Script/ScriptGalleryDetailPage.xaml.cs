using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using iHentai.Services.Models.Script;
using iHentai.ViewModels.Script;
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

        private void GoBackClicked(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void ReadClicked(object sender, RoutedEventArgs e)
        {
        }

        private async void ContentPresenter_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.Tag is ScriptGalleryChapter chapter &&
                ViewModel.Detail != null)
            {
                if (await ViewModel.CheckCanOpenChapter(chapter))
                {
                    this.FindAscendant<RootView>().ContentFrame.Navigate(typeof(ReadingPage),
                        new ScriptReadingViewModel(ViewModel.Api, ViewModel.Detail, chapter));
                }
            }
        }
    }
}