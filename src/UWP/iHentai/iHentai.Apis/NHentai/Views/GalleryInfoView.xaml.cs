using System.Linq;
using Windows.UI.Xaml;
using Humanizer;
using iHentai.Apis.NHentai.Models;
using iHentai.Services;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace iHentai.Apis.NHentai.Views
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
            LanguageTextBlock.Text = string.Empty;
            if (!(args.NewValue is GalleryModel))
                return;
            var model = (GalleryModel) args.NewValue;
            if (model.Tags == null)
                return;
            var tags = model.Tags?.GroupBy(item => item.Type).ToDictionary(item => item.Key, item => item.ToList());
            if (tags.TryGetValue("language", out var res))
                LanguageTextBlock.Text = res.LastOrDefault()?.Name?.Humanize(LetterCasing.Title) ?? string.Empty;
        }
    }
}