using Html2Model.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace iHentai.Services.EHentai.Models
{
    public class GalleryListModel
    {
        [HtmlMultiItems("[class^=gtr]")]
        public List<GalleryModel> Gallery { get; set; }
    }
}
