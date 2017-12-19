using Windows.UI.Xaml;
using iHentai.Apis.EHentai.Models;
using iHentai.Services;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace iHentai.Apis.EHentai.Views
{
    [ContentType("Detail")]
    public sealed partial class GalleryDetailView : IContentView<GalleryDetailModel>
    {
        public GalleryDetailView()
        {
            InitializeComponent();
        }

        private void Search_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ShowWiki_OnClick(object sender, RoutedEventArgs e)
        {
        }
    }
}