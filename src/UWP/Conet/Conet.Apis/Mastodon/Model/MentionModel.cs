using Newtonsoft.Json;

namespace Conet.Apis.Mastodon.Model
{
    public class MentionModel
    {
        /// <summary>
        /// URL of user's profile (can be remote)
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// The username of the account
        /// </summary>
        [JsonProperty("username")]
        public string UserName { get; set; }

        /// <summary>
        /// Equals username for local users, includes @domain for remote ones
        /// </summary>
        [JsonProperty("acct")]
        public string Acct { get; set; }

        /// <summary>
        /// Account ID
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
