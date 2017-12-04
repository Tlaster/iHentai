using iHentai.Apis.Core.Models.Interfaces;
using iHentai.Apis.EHentai.Models;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace iHentai.Apis.EHentai.Views
{
    [ContentType(ContentTypes.Info)]
    public sealed partial class GalleryInfoView : IGalleryContentView<GalleryModel>
    {
        public GalleryInfoView()
        {
            InitializeComponent();
        }
    }
}