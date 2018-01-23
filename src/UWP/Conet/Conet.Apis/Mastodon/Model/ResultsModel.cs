using System.Collections.Generic;
using Newtonsoft.Json;

namespace Conet.Apis.Mastodon.Model
{
    public class ResultsModel
    {
        /// <summary>
        /// An array of matched <see cref="AccountModel"/>
        /// </summary>
        [JsonProperty("accounts")]
        public List<AccountModel> Accounts { get; set; }

        /// <summary>
        /// An array of matchhed <see cref="Statuses"/>
        /// </summary>
        [JsonProperty("statuses")]
        public List<StatusModel> Statuses { get; set; }

        /// <summary>
        /// An array of matched hashtags, as strings
        /// </summary>
        [JsonProperty("hashtags")]
        public List<string> Hashtags { get; set; }
    }
}
