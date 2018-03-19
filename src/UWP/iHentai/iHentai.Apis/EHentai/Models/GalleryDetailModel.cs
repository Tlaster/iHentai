using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using Html2Model;
using Html2Model.Attributes;
using iHentai.Apis.Core.Models.Interfaces;
using iHentai.Basic.Extensions;

namespace iHentai.Apis.EHentai.Models
{
    public class GalleryDetailModel : IGalleryDetailModel
    {
        private string[] _information;

        [HtmlItem("#gd1 > div", Attr = "style", RegexPattern = "url\\((.*)\\)", RegexGroup = 1)]
        public string Cover { get; set; }

        [HtmlItem("#gn")]
        public string TitleEn { get; set; }

        [HtmlItem("#gj")]
        public string TitleJp { get; set; }

        [HtmlItem("#gdc > a", Attr = "href")]
        [HtmlConverter(typeof(CategoryConverter))]
        public CategoryFlags Category { get; set; }

        [HtmlItem("#rating_label", RegexPattern = "Average: (.*)", RegexGroup = 1)]
        public double Rating { get; set; }

        [HtmlItem("#rating_count")]
        public long RatingCount { get; set; }

        [HtmlItem("#gdn")]
        public string Uploader { get; set; }

        [HtmlItem("#gdn a", Attr = "href")]
        public string UploaderLink { get; set; }

        [HtmlMultiItems("#gdd > table > tbody > tr")]
        public string[] Information
        {
            get => _information;
            set
            {
                _information = value;
                try
                {
                    PublishAt = value.Where(item => Regex.IsMatch(item, "Posted:(.*)")).Select(item =>
                        DateTime.Parse(Regex.Match(item, "Posted:(.*)").Groups[1].Value.Trim())).FirstOrDefault();
                    FavoritedCount = value.Where(item => Regex.IsMatch(item, "Favorited:(.*)times"))
                        .Select(item => int.Parse(Regex.Match(item, "Favorited:(.*)times").Groups[1].Value.Trim()))
                        .FirstOrDefault();
                    FileSize = value.Where(item => Regex.IsMatch(item, "File Size:(.*)"))
                        .Select(item => Regex.Match(item, "File Size:(.*)").Groups[1].Value.Trim()).FirstOrDefault();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    Debug.WriteLine(e.StackTrace);
                }
            }
        }

        public string FileSize { get; set; }

        public DateTime PublishAt { get; set; }

        public long FavoritedCount { get; set; }

        [HtmlMultiItems("#taglist > table > tbody > tr")]
        public TagModel[] Tags { get; set; }

        [HtmlItem(".gpc", RegexPattern = "Showing (.*) of (.*) images", RegexGroup = 2)]
        public long TotalImages { get; set; }

        [HtmlItem("#gd5 > p:nth-child(3) > a", RegexPattern = "Torrent Download \\( (.*) \\)", RegexGroup = 1)]
        public int TorrentCount { get; set; }

        [HtmlMultiItems(".gdtl")]
        public ImageModel[] Images { get; set; }

        [HtmlItem(".ptt > tbody > tr > td:nth-last-child(2)")]
        public int MaxPage { get; set; }

        [HtmlMultiItems(".c1")]
        public CommentModel[] Comments { get; set; }
    }

    public class ImageModel
    {
        [HtmlItem("img", Attr = "src")]
        public string Link { get; set; }

        [HtmlItem("img", Attr = "alt")]
        public int Page { get; set; }
    }

    public class CommentModel
    {
        [HtmlItem(".c3", RegexPattern = "Posted on (.*) UTC by", RegexGroup = 1)]
        public DateTime CreatedAt { get; set; }

        [HtmlItem(".c3", RegexPattern = "by:(\\s*)(\\S*)", RegexGroup = 2)]
        public string UserName { get; set; }

        [HtmlItem(".c6")]
        [HtmlConverter(typeof(CommentContentConverter))]
        public string Content { get; set; }

        [HtmlItem(".c5")]
        public string Score { get; set; }

        [HtmlItem(".c7")]
        public string Vote { get; set; }

        public string[] VoteHistory => Vote.IsEmpty() ? default : Vote.Split(",").Select(item => item.Trim()).ToArray();
    }

    public class CommentContentConverter : IHtmlConverter
    {
        public object ReadHtml(INode node, Type targetType, object existingValue)
        {
            return (node as IElement).InnerHtml;
        }
    }

    public class TagModel : ITagModel
    {
        [HtmlItem(".tc")]
        public string Title { get; set; }

        [HtmlMultiItems("td > div > a")]
        public string[] Tags { get; set; }
    }
}