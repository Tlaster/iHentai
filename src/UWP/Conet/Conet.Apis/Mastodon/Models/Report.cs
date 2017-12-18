using Newtonsoft.Json;

namespace Conet.Apis.Mastodon.Models
{
    public class Report
    {
        [JsonProperty("id")]
        public long Id { get; set; }


        [JsonProperty("action_taken")]
        public string ActionTaken { get; set; }
    }
}