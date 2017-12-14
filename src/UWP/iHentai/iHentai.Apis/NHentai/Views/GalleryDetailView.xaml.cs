using iHentai.Apis.Core.Models.Interfaces;
using iHentai.Apis.NHentai.Models;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace iHentai.Apis.NHentai.Views
{
    [ContentType(ContentTypes.Detail)]
    public sealed partial class GalleryDetailView : IGalleryContentView<GalleryModel>
    {
        public GalleryDetailView()
        {
            InitializeComponent();
        }
    }
}