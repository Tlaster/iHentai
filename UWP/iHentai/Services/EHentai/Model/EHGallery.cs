using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using iHentai.Common.Html;
using iHentai.Common.Html.Attributes;
using iHentai.Services.Core;

namespace iHentai.Services.EHentai.Model
{
    internal class EHGalleryList
    {
        [HtmlMultiItems(".itg.gltc tr:not(:first-child)")]
        [HtmlMultiItems(".itg.gltm tr:not(:first-child)")]
        public List<EHGallery> Items { get; set; }
    }

    internal class EHGalleryImage
    {
        [HtmlItem("#img", Attr = "src")]
        public string Source { get; set; }
        
        [HtmlItem("#img", Attr = "style", RegexPattern = "height:(\\d+)", RegexGroup = 1)]
        public int Height { get; set; }

        [HtmlItem("#img", Attr = "style", RegexPattern = "width:(\\d+)", RegexGroup = 1)]
        public int Width { get; set; }

        [HtmlItem("#i7 > a", Attr = "href")]
        public string OriginalSource { get; set; }

        [HtmlItem("#loadfail", Attr = "onclick", RegexPattern = "\\((.*)\\)", RegexGroup = 1)] 
        public string LoadFailed { get; set; }
    }

    internal class EHGalleryDetail
    {
        [HtmlItem("#gn")] public string Name { get; set; }

        [HtmlItem("#gj")] public string NameJP { get; set; }

        [HtmlMultiItems("#taglist > table > tbody > tr")]
        public List<EHGalleryTagCategory> Tags { get; set; }

        [HtmlItem("#gdc")]
        [HtmlConverter(typeof(CategoryConverter))]
        public EHCategory Category { get; set; }

        [HtmlItem("#gdn > a")] public string Uploader { get; set; }

        [HtmlItem("#gdn > a", Attr = "href")] public string UploaderLink { get; set; }

        [HtmlItem("#gd1 > div", Attr = "style", RegexPattern = "url\\(([^\\s]*)\\)", RegexGroup = 1)]
        public string Thumb { get; set; }
        
        [HtmlItem("#gd1 > div", Attr = "style", RegexPattern = "height:(\\d+)", RegexGroup = 1)]
        public int ThumbHeight { get; set; }

        [HtmlItem("#gd1 > div", Attr = "style", RegexPattern = "width:(\\d+)", RegexGroup = 1)]
        public int ThumbWidth { get; set; }

        [HtmlItem("#rating_image", Attr = "style")]
        [HtmlConverter(typeof(RatingConverter))]
        public double Rating { get; set; }

        [HtmlItem("#rating_count")] public long RatingCount { get; set; }

        [HtmlItem("#rating_label", RegexPattern = "(\\d+(\\.\\d+)?)", RegexGroup = 1)]
        public double RatingAverage { get; set; }

        [HtmlMultiItems(".gdtl")] public List<EHGalleryPageLargeImage> LargeImages { get; set; }

        [HtmlMultiItems(".gdtm")] public List<EHGalleryPageNormalImage> NormalImages { get; set; }

        [HtmlMultiItems(".c1")] public List<EHGalleryComment> Comments { get; set; }

        [HtmlMultiItems("#gdd tr")]
        public List<EHGalleryInformation> Information { get; set; }

        [HtmlMultiItems(".ptt td:not(:first-child):not(:last-child) > a", Attr = "href")]
        public List<string> Pages { get; set; }
    }

    internal class EHGalleryInformation
    {
        [HtmlItem(".gdt1")]
        public string Title { get; set; }

        [HtmlItem(".gdt2")]
        public string Value { get; set; }
    }

    internal class EHGalleryComment
    {
        [HtmlItem(".c3", RegexPattern = "Posted on (.*) by:", RegexGroup = 1)] public string CreatedAt { get; set; }

        [HtmlItem(".c6", RawHtml = true)] public string Content { get; set; }

        [HtmlItem(".c7")] public string Votes { get; set; }

        [HtmlItem(".c3 > a")] public string User { get; set; }

        [HtmlItem(".c5 > span")] public string Point { get; set; }
    }

    internal interface IEHGalleryImage
    {
        string Text { get; }
        string Link { get; }
    }

    internal class EHGalleryPageNormalImage : IEHGalleryImage
    {
        [HtmlItem("a", Attr = "href")] public string Link { get; set; }

        [HtmlItem("div", Attr = "style", RegexPattern = "url\\(([^\\s]*)\\)", RegexGroup = 1)]
        public string Source { get; set; }

        [HtmlItem("div", Attr = "style", RegexPattern = "url\\(([^\\s]*)\\) ((-)?\\d+)(px)?", RegexGroup = 2)]
        public double OffsetX { get; set; }

        [HtmlItem("div", Attr = "style", RegexPattern = "url\\(([^\\s]*)\\) ((-)?\\d+)(px)? ((-)?\\d+)(px)?",
            RegexGroup = 5)]
        public double OffsetY { get; set; }
        
        [HtmlItem("img", Attr = "style", RegexPattern = "height:(\\d+)", RegexGroup = 1)]
        public int ThumbHeight { get; set; }

        [HtmlItem("img", Attr = "style", RegexPattern = "width:(\\d+)", RegexGroup = 1)]
        public int ThumbWidth { get; set; }

        [HtmlItem("img", Attr = "alt")] public string Text { get; set; }
    }

    internal class EHGalleryPageLargeImage : IEHGalleryImage
    {
        [HtmlItem("a", Attr = "href")] public string Link { get; set; }

        [HtmlItem("img", Attr = "src")] public string Source { get; set; }
        
        [HtmlItem("img", Attr = "alt")] public string Text { get; set; }
    }

    internal class EHGalleryTagCategory
    {
        [HtmlItem(".tc")] public string Name { get; set; }

        [HtmlMultiItems("td:nth-child(2) > div")] public List<EHGalleryTag> Tags { get; set; }
    }

    internal class EHGalleryTag
    {
        [HtmlItem("a")] public string Name { get; set; }

        [HtmlItem("a", Attr = "href")] public string Link { get; set; }
    }

    internal class EHGallery : IGallery
    {
        [HtmlItem(".glname a", Attr = "href")] public string Link { get; set; }

        [HtmlItem(".glthumb img", Attr = "style", RegexPattern = "height:(\\d+)", RegexGroup = 1)]
        public int ThumbHeight { get; set; }

        [HtmlItem(".glthumb img", Attr = "style", RegexPattern = "width:(\\d+)", RegexGroup = 1)]
        public int ThumbWidth { get; set; }

        [HtmlItem(".gl2c > div:nth-child(3) > div:nth-child(1)")]
        public DateTime PublishAt { get; set; }

        [HtmlItem(".itd .it4 div", Attr = "style")]
        [HtmlConverter(typeof(RatingConverter))]
        public double Rating { get; set; }

        [HtmlItem(".gl4c.glhide a")] public string Uploader { get; set; }

        [HtmlItem(".gl4c.glhide a", Attr = "href")]
        public string UploaderLink { get; set; }

        [HtmlItem(".cn")]
        [HtmlConverter(typeof(CategoryConverter))]
        public EHCategory Category { get; set; }

        [HtmlItem(".gldown a", Attr = "href")] public string TorrentLink { get; set; }

        [HtmlItem(".glink")] public string Title { get; set; }

        [HtmlItem(".glthumb img", Attr = "data-src")]
        [HtmlItem(".glthumb img", Attr = "src")]
        public string Thumb { get; set; }
    }

    internal enum EHCategory
    {
        Doujinshi,
        Manga,
        ArtistCG,
        GameCG,
        Western,
        NonH,
        ImageSet,
        Cosplay,
        AsianPorn,
        Misc
    }

    internal class CategoryConverter : IHtmlConverter
    {
        public object ReadHtml(INode node, Type targetType, object existingValue)
        {
            return node.TextContent.Trim() switch
            {
                "Doujinshi" => EHCategory.Doujinshi,
                "Manga" => EHCategory.Manga,
                "Artist CG" => EHCategory.ArtistCG,
                "Game CG" => EHCategory.GameCG,
                "Western" => EHCategory.Western,
                "Non-H" => EHCategory.NonH,
                "Image Set" => EHCategory.ImageSet,
                "Cosplay" => EHCategory.Cosplay,
                "Asian Porn" => EHCategory.AsianPorn,
                "Misc" => EHCategory.Misc,
#if DEBUG
                _ => throw new ArgumentOutOfRangeException(nameof(node.TextContent), node.TextContent, null)
#else
                _ => EHCategory.Doujinshi
#endif
            };
        }
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