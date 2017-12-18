using Newtonsoft.Json;

namespace Conet.Apis.Sina.Models
{
    public class PicUrl
    {
        [JsonProperty("thumbnail_pic")]
        public string ThumbnailPic { get; set; }
    }
}