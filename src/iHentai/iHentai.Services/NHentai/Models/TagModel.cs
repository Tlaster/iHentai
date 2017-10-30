using Newtonsoft.Json;

namespace iHentai.Services.NHentai.Models
{
    public class TagModel
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}