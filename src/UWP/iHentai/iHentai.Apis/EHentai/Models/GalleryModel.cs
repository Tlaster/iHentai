using System;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using Html2Model;
using Html2Model.Attributes;
using iHentai.Apis.Core.Models.Interfaces;
using iHentai.Apis.EHentai.Models.HtmlConverters;

namespace iHentai.Apis.EHentai.Models
{
    public class GalleryModel : IGalleryModel
    {
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
        public long ID { get; set; }

        [HtmlItem(".it5 a", Attr = "href", RegexPattern = "/g/([^/]+)/([^-]+)/", RegexGroup = 2)]
        public string Token { get; set; }

        [HtmlItem(".itu")]
        public string Uploader { get; set; }

        [HtmlItem(".itu div a", Attr = "href")]
        public string UploaderLink { get; set; }

        [HtmlItem(".it5")]
        public string Title { get; set; }

//        [HtmlItem(".it2 img", Attr = "src")]
        [HtmlItem(".it2")]
        [HtmlConverter(typeof(ThumbConverter))]
        public string Thumb { get; set; }

        [HtmlItem(".it2", Attr = "style")]
        [HtmlConverter(typeof(ThumbHeightConverter))]
        public double ThumbHeight { get; set; }

        [HtmlItem(".it2", Attr = "style")]
        [HtmlConverter(typeof(ThumbWidthConverter))]
        public double ThumbWidth { get; set; }
    }

    internal class ThumbConverter : IHtmlConverter
    {
        public object ReadHtml(INode node, Type targetType, object existingValue)
        {
            if (node.HasChildNodes && node.ChildNodes[0] is IHtmlImageElement element)
                return element.Attributes["src"].Value;
            var text = (node as IElement).TextContent;
            var match = Regex.Match(text, "inits~([^~]*)~([^~]*)~");
            return $"https://{match.Groups[1].Value}/{match.Groups[2].Value}";
        }
    }

    internal class ThumbHeightConverter : IHtmlConverter
    {
        public object ReadHtml(INode node, Type targetType, object existingValue)
        {
            var text = (node as IElement).GetAttribute("style").ToLowerInvariant();
            return int.TryParse(Regex.Match(text, "height:(.*)px;").Groups[1].Value, out var res) ? res : 0;
        }
    }


    internal class ThumbWidthConverter : IHtmlConverter
    {
        public object ReadHtml(INode node, Type targetType, object existingValue)
        {
            var text = (node as IElement).GetAttribute("style").ToLowerInvariant();
            return int.TryParse(Regex.Match(text, "width:(.*)px").Groups[1].Value, out var res) ? res : 0;
        }
    }
}