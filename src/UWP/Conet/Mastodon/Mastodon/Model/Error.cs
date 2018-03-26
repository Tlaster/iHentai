using Newtonsoft.Json;

namespace Mastodon.Model
{
    public class Error
    {
        /// <summary>
        ///     A textual description of the error
        /// </summary>
        [JsonProperty("error")]
        public string Description { get; set; }
    }
}