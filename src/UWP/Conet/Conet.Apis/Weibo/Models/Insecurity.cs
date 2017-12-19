using Newtonsoft.Json;

namespace Conet.Apis.Weibo.Models
{
    public class Insecurity
    {
        [JsonProperty("sexual_content")]
        public bool SexualContent { get; set; }
    }
}