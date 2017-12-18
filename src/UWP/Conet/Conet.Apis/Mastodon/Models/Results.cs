using System.Collections.Generic;
using Newtonsoft.Json;

namespace Conet.Apis.Mastodon.Models
{
    public class Results
    {
        [JsonProperty("accounts")]
        public IEnumerable<Account> Accounts { get; set; }


        [JsonProperty("statuses")]
        public IEnumerable<Status> Statuses { get; set; }


        [JsonProperty("hashtags")]
        public IEnumerable<string> Hashtags { get; set; }
    }
}