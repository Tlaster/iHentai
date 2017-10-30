using System;
using Html2Model;
using Html2Model.Attributes;
using iHentai.Services.Core.Models.Interfaces;
using iHentai.Services.EHentai.Models.HtmlConverters;

namespace iHentai.Services.EHentai.Models
{
    public class GalleryModel : IGalleryModel
    {
        [HtmlItem(".it5")]
        public string Title { get; set; }

        [HtmlItem(".it2 img", Attr = "src")]
        public string Thumb { get; set; }

        [HtmlItem(".it5 a", Attr = "href")]
        public string Link { get; set; }

        [HtmlItem(".itd")]
        public DateTime PublishAt { get; set; }

        [HtmlItem(".ic", Attr = "alt")]
        [HtmlConverter(typeof(CategoryConverter))]
        public CategoryFlags Category { get; set; }

        [HtmlItem(".itd .it4 div", Attr = "style")]
        [HtmlConverter(typeof(RatingConverter))]
        public double Rating { get; set; }

        [HtmlItem(".it5 a", Attr = "href", RegexPattern = "/g/([^/]+)/([^-]+)/", RegexGroup = 1)]
        public int ID { get; set; }

        [HtmlItem(".it5 a", Attr = "href", RegexPattern = "/g/([^/]+)/([^-]+)/", RegexGroup = 2)]
        public string Token { get; set; }

        [HtmlItem(".itu")]
        public string Uploader { get; set; }

        [HtmlItem(".itu div a", Attr = "href")]
        public string UploaderLink { get; set; }
    }
}