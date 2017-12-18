using Newtonsoft.Json;

namespace Conet.Apis.Mastodon.Models
{
    public class Instance
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }


        [JsonProperty("title")]
        public string Title { get; set; }


        [JsonProperty("description")]
        public string Description { get; set; }


        [JsonProperty("email")]
        public string Email { get; set; }
    }
}