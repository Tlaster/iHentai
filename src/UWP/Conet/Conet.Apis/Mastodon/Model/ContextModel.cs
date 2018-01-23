using System.Collections.Generic;
using Newtonsoft.Json;

namespace Conet.Apis.Mastodon.Model
{
    public class ContextModel
    {
        /// <summary>
        /// The ancestors of the status in the conversation, as a list of <see cref="StatusModel"/>
        /// </summary>
        [JsonProperty("ancestors")]
        public List<StatusModel> Ancestors { get; set; }

        /// <summary>
        /// The descendants of the status in the conversation, as a list of <see cref="StatusModel"/>
        /// </summary>
        [JsonProperty("descendants")]
        public List<StatusModel> Descendants { get; set; }
    }
}
