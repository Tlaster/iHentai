using Newtonsoft.Json;

namespace Conet.Apis.Sina.Models
{
    public class ExtendInfo
    {
        [JsonProperty("ad")]
        public Ad Ad { get; set; }
    }
}