﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHentai.Common.Html.Attributes;
using iHentai.Services.Core;

namespace iHentai.Services.Manhuagui.Model
{
    class ManhuaguiGallery : IGallery
    {
        [HtmlItem("h3")]
        public string Title { get; set; }
        
        [HtmlItem(".thumb > img", Attr = "data-src")]
        public string Thumb { get; set; }

        [HtmlItem("a", Attr = "href")]
        public string Link { get; set; }

        [HtmlItem("dl:last-child > :last-child")]
        public string Date { get; set; }
    }

    class ManhuaguiGalleryDetail
    {
        [HtmlItem(".main-bar > h1")]
        public string Title { get; set; }

        [HtmlItem(".thumb > img", Attr = "src")]
        public string Image { get; set; }

        [HtmlItem("#bookIntro")]
        public string Desc { get; set; }
        
        [HtmlMultiItems("#chapterList>ul>li")]
        public List<ManhuaguGalleryChapter> Chapters { get; set; }
    }

    class ManhuaguGalleryChapter
    {
        [HtmlItem("a", Attr = "href")]
        public string Link { get; set; }

        [HtmlItem("a > b")]
        public string Title { get; set; }
    }
}