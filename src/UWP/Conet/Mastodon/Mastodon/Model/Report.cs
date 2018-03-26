using Newtonsoft.Json;

namespace Mastodon.Model
{
    public class Report
    {
        /// <summary>
        ///     The ID of the report
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        ///     The action taken in response to the report
        /// </summary>
        [JsonProperty("action_taken")]
        public string ActionTaken { get; set; }
    }
}