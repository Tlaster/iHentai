using Newtonsoft.Json;

namespace iHentai.Extensions.Models
{
    public class NetworkExtensionModel
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("src")]
        public string Src { get; set; }

        [JsonProperty("version")] 
        public int Version { get; set; }
    }
}