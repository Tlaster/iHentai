using Newtonsoft.Json;

namespace Conet.Apis.Mastodon.Models
{
    public class Error
    {
        [JsonProperty("error")]
        public string Description { get; set; }
    }
}