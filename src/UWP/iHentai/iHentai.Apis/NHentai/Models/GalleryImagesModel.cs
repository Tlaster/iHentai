using Newtonsoft.Json;

namespace iHentai.Apis.NHentai.Models
{
    public class GalleryImagesModel
    {
        [JsonProperty("cover")]
        public ImageModel Cover { get; set; }

        [JsonProperty("pages")]
        public ImageModel[] Pages { get; set; }

        [JsonProperty("thumbnail")]
        public ImageModel Thumbnail { get; set; }
    }
}