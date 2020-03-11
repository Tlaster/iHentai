using Windows.UI.Xaml;
using iHentai.Services.EHentai.Model;
using iHentai.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Activities.EHentai
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    partial class GalleryActivity
    {
        GalleryViewModel ViewModel { get; } = new GalleryViewModel();

        public GalleryActivity()
        {
            this.InitializeComponent();
        }

        private async void AspectRatioView_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.Tag is EHGallery gallery)
            {
                e.Handled = true;
            }
        }
    }
}
