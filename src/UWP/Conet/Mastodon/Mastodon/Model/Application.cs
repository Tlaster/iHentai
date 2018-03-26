using Newtonsoft.Json;

namespace Mastodon.Model
{
    public class Application
    {
        /// <summary>
        ///     Name of the app
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        ///     Homepage URL of the app
        /// </summary>
        [JsonProperty("website")]
        public string Website { get; set; }
    }
}