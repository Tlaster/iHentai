using Newtonsoft.Json;

namespace Conet.Apis.Weibo.Models
{
    public class ExtendInfo
    {
        [JsonProperty("ad")]
        public Ad Ad { get; set; }
    }
}