using Newtonsoft.Json;

namespace Conet.Apis.Sina.Models
{
    public class Insecurity
    {
        [JsonProperty("sexual_content")]
        public bool SexualContent { get; set; }
    }
}