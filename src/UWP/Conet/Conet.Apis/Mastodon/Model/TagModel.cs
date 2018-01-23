using Newtonsoft.Json;

namespace Conet.Apis.Mastodon.Model
{
    public class TagModel
    {
        /// <summary>
        /// The hashtag, not including the preceding #
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The URL of the hashtag
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
