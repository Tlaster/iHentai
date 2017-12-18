using System;
using Newtonsoft.Json;

namespace Conet.Apis.Mastodon.Models
{
    public class Notification
    {
        [JsonProperty("id")]
        public long Id { get; set; }


        [JsonProperty("type")]
        public string Type { get; set; }


        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }


        [JsonProperty("account")]
        public Account Account { get; set; }


        [JsonProperty("status")]
        public Status Status { get; set; }
    }
}