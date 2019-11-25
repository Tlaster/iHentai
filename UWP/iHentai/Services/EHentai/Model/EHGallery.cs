using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Dom;
using iHentai.Common.Html;
using iHentai.Common.Html.Attributes;
using iHentai.Services.Core;

namespace iHentai.Services.EHentai.Model
{
    class EHGallery : IGallery
    {
        [HtmlItem(".glink")]
        public string Title { get; set; }
        [HtmlItem(".glthumb img", Attr = "data-src")]
        public string Thumb { get; set; }
        [HtmlItem(".glname a", Attr = "href")]
        public string Link { get; set; }
        [HtmlItem(".glthumb img", Attr = "style", RegexPattern = "height:(\\d+)", RegexGroup = 1)]
        public int ThumbHeight { get; set; }
        [HtmlItem(".glthumb img", Attr = "style", RegexPattern = "width:(\\d+)", RegexGroup = 1)]
        public int ThumbWidth { get; set; }
        [HtmlItem(".gl2c > div:nth-child(3) > div:nth-child(1)")]
        public DateTime PublishAt { get; set; }
        [HtmlItem(".itd .it4 div", Attr = "style")]
        [HtmlConverter(typeof(RatingConverter))]
        public double Rating { get; set; }
        [HtmlItem(".gl4c.glhide a")]
        public string Uploader { get; set; }
        [HtmlItem(".gl4c.glhide a", Attr = "href")]
        public string UploaderLink { get; set; }
        [HtmlItem(".cn")]
        public string Category { get; set; }
        [HtmlItem(".gldown a", Attr = "href")]
        public string TorrentLink { get; set; }
    }

    internal class RatingConverter : IHtmlConverter
    {
        public object ReadHtml(INode node, Type targetType, object existingValue)
        {
            var match = Regex.Match((node as IElement).GetAttribute("style"),
                "background-position:-?(\\d+)px -?(\\d+)px");
            var num1 = Convert.ToInt32(match.Groups[1].Value);
            var num2 = Convert.ToInt32(match.Groups[2].Value);
            var rate = 5d;
            rate = rate - num1 / 16d;
            return num2 == 21 ? --rate + 0.5 : rate;
        }
    }
}
