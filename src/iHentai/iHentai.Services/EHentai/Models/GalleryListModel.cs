using System.Collections.Generic;
using Html2Model.Attributes;

namespace iHentai.Services.EHentai.Models
{
    public class GalleryListModel
    {
        [HtmlItem(".ptt > tbody > tr > td:nth-last-child(2) > a")]
        public int MaxPage { get; set; }

        [HtmlMultiItems("[class^=gtr]")]
        public List<GalleryModel> Gallery { get; set; }
    }
}