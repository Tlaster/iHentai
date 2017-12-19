

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Humanizer;
using iHentai.Apis.Core.Models.Interfaces;
using iHentai.Apis.EHentai.Converters;
using iHentai.Apis.EHentai.Models;
using iHentai.Basic.Helpers;
using iHentai.Services;

namespace iHentai.Apis.EHentai.Views
{
    [ContentType("DetailInfo")]
    public sealed partial class GalleryDetailInfoView : IContentView<GalleryDetailModel>
    {
        public GalleryDetailInfoView()
        {
            InitializeComponent();
        }

        private void GalleryDetailInfoView_OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue is GalleryDetailModel gallery)
            {
                var color = gallery.Category.ToColor();
                CategoryNameBorder.RequestedTheme =
                    ThemeHelper.CheckColorIsLight(color) ? ElementTheme.Light : ElementTheme.Dark;
                CategoryNameBorder.Background = new SolidColorBrush(color);
                CategoryName.Text = gallery.Category.ToString().Humanize(LetterCasing.AllCaps);
            }
        }
    }
}