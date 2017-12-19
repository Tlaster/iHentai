using Newtonsoft.Json;

namespace Conet.Apis.Weibo.Models
{
    public class Visible
    {
        [JsonProperty("type")]
        public long Type { get; set; }

        [JsonProperty("list_id")]
        public long ListId { get; set; }
    }
}