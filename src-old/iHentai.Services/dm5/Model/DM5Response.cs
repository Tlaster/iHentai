using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHentai.Html.Attributes;
using iHentai.Services.Core;
using Newtonsoft.Json;

namespace iHentai.Services.dm5.Model
{
    public partial class Dm5Response
    {
        [JsonProperty("UpdateComicItems")]
        public List<DM5Gallery> UpdateComicItems { get; set; }

        [JsonProperty("Count")]
        public long Count { get; set; }
    }

    
    public partial class DM5SearchGallery : IMangaGallery
    {
        [JsonProperty("Id")]
        public long Id { get; set; }

        [JsonProperty("Author")]
        public List<string> Author { get; set; }

        [JsonProperty("Url")]
        public string Url { get; set; }

        [JsonProperty("Pic")]
        public string Pic { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        public string Thumb => Pic;

        [JsonProperty("LastUpdateInfo")]
        public string LastUpdateInfo { get; set; }

        [JsonProperty("LastPartShowName")]
        public string LastPartShowName { get; set; }

        [JsonProperty("Mid")]
        public long Mid { get; set; }

        [JsonProperty("ReadUrl")]
        public string ReadUrl { get; set; }

        [JsonProperty("BigPic")]
        public string BigPic { get; set; }

        [JsonProperty("Content")]
        public string Content { get; set; }

        [JsonProperty("TagList")]
        public List<string> TagList { get; set; }

        [JsonProperty("LastPartUrl")]
        public string LastPartUrl { get; set; }

        [JsonProperty("AuthorUrl")]
        public List<string> AuthorUrl { get; set; }

        [JsonProperty("LastPartTime")]
        public DateTimeOffset LastPartTime { get; set; }

        [JsonProperty("AddTime")]
        public DateTimeOffset AddTime { get; set; }

        [JsonProperty("Categorys")]
        public string Categorys { get; set; }

        [JsonProperty("Score")]
        public object Score { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }

        public string UpdateAt => LastPartTime.ToString("yyyy MMMM dd");
        public string Chapter => LastUpdateInfo;
    }


    public partial class DM5Gallery : IMangaGallery
    {
        [JsonProperty("ID")]
        public long Id { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        public string Thumb => ShowPicUrlB;

        [JsonProperty("UrlKey")]
        public string UrlKey { get; set; }

        [JsonProperty("Logo")]
        public string Logo { get; set; }

        [JsonProperty("LastPartUrl")]
        public string LastPartUrl { get; set; }

        [JsonProperty("ShowLastPartName")]
        public string ShowLastPartName { get; set; }

        [JsonProperty("ShowPicUrlB")]
        public string ShowPicUrlB { get; set; }

        [JsonProperty("ShowConver")]
        public string ShowConver { get; set; }

        [JsonProperty("ComicPart")]
        public string ComicPart { get; set; }

        [JsonProperty("Author")]
        public List<string> Author { get; set; }

        [JsonProperty("ShowReads")]
        public string ShowReads { get; set; }

        [JsonProperty("Content")]
        public string Content { get; set; }

        [JsonProperty("Star")]
        public long Star { get; set; }

        [JsonProperty("ShowSource")]
        public object ShowSource { get; set; }

        [JsonProperty("Status")]
        public long Status { get; set; }

        [JsonProperty("LastUpdateTime")]
        public string LastUpdateTime { get; set; }

        [JsonProperty("ShelvesTime")]
        public string ShelvesTime { get; set; }

        public string UpdateAt => LastUpdateTime;
        public string Chapter => ShowLastPartName;
    }


    class DM5GalleryDetail : IMangaDetail
    {
        [HtmlItem(".detail-main-info-title")]
        public string Title { get; set; }
        [HtmlItem(".detail-main-cover > img", Attr = "src")]
        public string Thumb { get; set; }
        [HtmlItem(".detail-desc")]
        public string Desc { get; set; }
        [HtmlMultiItems("#detail-list-select-1>li")]
        public List<DM5GalleryChapter> Chapters { get; set; }

        IEnumerable<IMangaChapter> IMangaDetail.Chapters => Chapters;
    }

    class DM5GalleryChapter : IMangaChapter
    {
        [HtmlItem("a", Attr = "href")]
        public string Link { get; set; }
        [HtmlItem(".detail-list-2-info-title")]
        [HtmlItem("a")]
        public string Title { get; set; }
        [HtmlItem(".detail-list-2-info-subtitle")]
        public string SubTitle { get; set; }
        public bool Updated { get; set; }
        [HtmlItem(".detail-list-2-info-right")]
        [HtmlConverter(typeof(NullToBoolHtmlConverter))]
        public bool IsLocked { get; set; }
        [HtmlItem(".detail-list-2-cover-img", Attr = "src")]
        public string Thumb { get; set; }
    }
}
