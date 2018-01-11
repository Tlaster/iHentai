using iHentai.Apis.NHentai.Models;
using iHentai.Services;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace iHentai.Apis.NHentai.Views
{
    [ContentKey("DetailInfo")]
    public sealed partial class GalleryDetailInfoView : IContentView<GalleryModel>
    {
        public GalleryDetailInfoView()
        {
            InitializeComponent();
        }
    }
}