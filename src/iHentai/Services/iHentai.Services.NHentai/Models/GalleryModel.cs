using Html2Model.Attributes;
using iHentai.Services.Core.Models.Interfaces;

namespace iHentai.Services.Core.iHentai.Services.NHentai.Models
{
    public class GalleryModel : IGalleryModel
    {
        [HtmlItem(".caption")]
        public string Title { get; set; }
        [HtmlItem("a > img", Attr = "src")]
        public string Thumb { get; set; }
        [HtmlItem("a", Attr = "href")]
        public string Link { get; set; }
    }
}