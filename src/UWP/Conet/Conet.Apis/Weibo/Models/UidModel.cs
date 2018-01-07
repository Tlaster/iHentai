using Newtonsoft.Json;

namespace Conet.Apis.Weibo.Models
{
    public class UidModel
    {
        [JsonProperty("uid")]
        public long Uid { get; set; }
    }
}