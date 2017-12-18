using Newtonsoft.Json;

namespace Conet.Apis.Mastodon.Models
{
    public class Application
    {
        [JsonProperty("name")]
        public string Name { get; set; }


        [JsonProperty("website")]
        public string Website { get; set; }
    }
}