using Newtonsoft.Json;

namespace Conet.Apis.Mastodon.Model
{
    public class ApplicationModel
    {
        /// <summary>
        /// Name of the app
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Homepage URL of the app
        /// </summary>
        [JsonProperty("website")]
        public string Website { get; set; }
    }
}
