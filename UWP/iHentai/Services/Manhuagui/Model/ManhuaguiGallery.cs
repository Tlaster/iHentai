using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHentai.Common.Html.Attributes;
using iHentai.Services.Core;

namespace iHentai.Services.Manhuagui.Model
{
    class ManhuaguiGalleryUpdate
    {
        [HtmlMultiItems(".latest-list > ul > li")]
        public List<ManhuaguiGallery> Items { get; set; }
    }
    class ManhuaguiGallery : IGallery
    {
        [HtmlItem(".ell")]
        public string Title { get; set; }
        
        [HtmlItem(".cover > img", Attr = "src")]
        [HtmlItem(".cover > img", Attr = "data-src")]
        public string Thumb { get; set; }

        [HtmlItem(".cover", Attr = "href")]
        public string Link { get; set; }

        [HtmlItem(".tt")]
        public string Desc { get; set; }

        [HtmlItem(".dt")]
        public string Date { get; set; }
    }
}
