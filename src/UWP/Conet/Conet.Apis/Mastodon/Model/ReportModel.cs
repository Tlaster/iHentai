using Newtonsoft.Json;

namespace Conet.Apis.Mastodon.Model
{
    public class ReportModel
    {
        /// <summary>
        /// The ID of the report
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// The action taken in response to the report
        /// </summary>
        [JsonProperty("action_taken")]
        public string ActionTaken { get; set; }
    }
}
