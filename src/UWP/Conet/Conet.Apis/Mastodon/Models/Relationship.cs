using Newtonsoft.Json;

namespace Conet.Apis.Mastodon.Models
{
    public class Relationship
    {
        [JsonProperty("id")]
        public long Id { get; set; }


        [JsonProperty("following")]
        public bool Following { get; set; }


        [JsonProperty("followed_by")]
        public bool FollowedBy { get; set; }


        [JsonProperty("blocking")]
        public bool Blocking { get; set; }


        [JsonProperty("muting")]
        public bool Muting { get; set; }


        [JsonProperty("requested")]
        public bool Requested { get; set; }


        [JsonProperty("domain_blocking")]
        public bool DomainBlocking { get; set; }
    }
}