using Newtonsoft.Json;

namespace Conet.Apis.Weibo.Models
{
    public class PicUrl
    {
        [JsonProperty("thumbnail_pic")]
        public string ThumbnailPic { get; set; }
    }
}