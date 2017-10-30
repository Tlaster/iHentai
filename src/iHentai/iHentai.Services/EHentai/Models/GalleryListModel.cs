using System.Collections.Generic;
using Html2Model.Attributes;

namespace iHentai.Services.EHentai.Models
{
    public class GalleryListModel
    {
        [HtmlMultiItems("[class^=gtr]")]
        public List<GalleryModel> Gallery { get; set; }
    }
}