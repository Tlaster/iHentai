using System.Collections.Generic;
using Newtonsoft.Json;

namespace Conet.Apis.Mastodon.Models
{
    public class Context
    {
        [JsonProperty("ancestors")]
        public IEnumerable<Status> Ancestors { get; set; }


        [JsonProperty("descendants")]
        public IEnumerable<Status> Descendants { get; set; }
    }
}