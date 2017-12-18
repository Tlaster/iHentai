using Newtonsoft.Json;

namespace Conet.Apis.Mastodon.Models
{
    public class Card
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }


        [JsonProperty("description")]
        public string Description { get; set; }


        [JsonProperty("image")]
        public string Image { get; set; }


        [JsonProperty("type")]
        public string Type { get; set; }


        [JsonProperty("author_name")]
        public string AuthorName { get; set; }


        [JsonProperty("author_url")]
        public string AuthorUrl { get; set; }


        [JsonProperty("provider_name")]
        public string ProviderName { get; set; }


        [JsonProperty("provider_url")]
        public string ProviderUrl { get; set; }


        [JsonProperty("html")]
        public string Html { get; set; }


        [JsonProperty("width")]
        public int? Width { get; set; }


        [JsonProperty("height")]
        public int? Height { get; set; }
    }
}