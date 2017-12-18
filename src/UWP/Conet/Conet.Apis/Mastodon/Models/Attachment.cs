using Newtonsoft.Json;

namespace Conet.Apis.Mastodon.Models
{
    public class Attachment
    {
        [JsonProperty("id")]
        public long Id { get; set; }


        [JsonProperty("type")]
        public string Type { get; set; }


        [JsonProperty("url")]
        public string Url { get; set; }


        [JsonProperty("remote_url")]
        public string RemoteUrl { get; set; }


        [JsonProperty("preview_url")]
        public string PreviewUrl { get; set; }


        [JsonProperty("text_url")]
        public string TextUrl { get; set; }
    }
}