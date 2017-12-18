using Newtonsoft.Json;

namespace Conet.Apis.Mastodon.Models
{
    public class Mention
    {
        [JsonProperty("id")]
        public long Id { get; set; }


        [JsonProperty("url")]
        public string Url { get; set; }


        [JsonProperty("username")]
        public string UserName { get; set; }


        [JsonProperty("acct")]
        public string AccountName { get; set; }
    }
}