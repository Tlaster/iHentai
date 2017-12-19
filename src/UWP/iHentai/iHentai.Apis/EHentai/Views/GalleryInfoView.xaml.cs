using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Humanizer;
using iHentai.Apis.EHentai.Converters;
using iHentai.Apis.EHentai.Models;
using iHentai.Basic.Helpers;
using iHentai.Services;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace iHentai.Apis.EHentai.Views
{
    [ContentType("Info")]
    public sealed partial class GalleryInfoView : IContentView<GalleryModel>
    {
        public GalleryInfoView()
        {
            InitializeComponent();
        }

        private void GalleryInfoView_OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue is GalleryModel gallery)
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