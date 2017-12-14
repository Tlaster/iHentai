using Newtonsoft.Json;

namespace iHentai.Apis.NHentai.Models
{
    public class GalleryListModel
    {
        [JsonProperty("result")]
        public GalleryModel[] Gallery { get; set; }

        [JsonProperty("num_pages")]
        public int NumPages { get; set; }

        [JsonProperty("per_page")]
        public int PerPage { get; set; }
    }
}