using Newtonsoft.Json;

namespace Conet.Apis.Mastodon.Model
{
    public class RelationshipModel
    {
        /// <summary>
        /// Target account id
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Whether the user is currently following the account
        /// </summary>
        [JsonProperty("following", NullValueHandling = NullValueHandling.Ignore)]
        public bool Following { get; set; }

        /// <summary>
        /// Whether the user is currently being followed by the account
        /// </summary>
        [JsonProperty("followed_by", NullValueHandling = NullValueHandling.Ignore)]
        public bool FollowedBy { get; set; }

        /// <summary>
        /// Whether the user is currently blocking the account
        /// </summary>
        [JsonProperty("blocking", NullValueHandling = NullValueHandling.Ignore)]
        public bool Blocking { get; set; }

        /// <summary>
        /// Whether the user is currently muting the account
        /// </summary>
        [JsonProperty("muting", NullValueHandling = NullValueHandling.Ignore)]
        public bool Muting { get; set; }

        /// <summary>
        /// Whether the user has requested to follow the account
        /// </summary>
        [JsonProperty("requested", NullValueHandling = NullValueHandling.Ignore)]
        public bool Requested { get; set; }
    }
}
