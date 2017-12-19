using Newtonsoft.Json;

namespace Conet.Apis.Weibo.Models
{
    public class Ad
    {
        [JsonProperty("url_marked")]
        public string UrlMarked { get; set; }
    }
}