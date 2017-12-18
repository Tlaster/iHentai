using Newtonsoft.Json;

namespace Conet.Apis.Sina.Models
{
    public class Annotation
    {
        [JsonProperty("mapi_request")]
        public bool? MapiRequest { get; set; }

        [JsonProperty("client_mblogid")]
        public string ClientMblogid { get; set; }
    }
}