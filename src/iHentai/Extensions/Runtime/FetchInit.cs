using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHentai.Scripting.Runtime;
using Newtonsoft.Json;

namespace iHentai.Extensions.Runtime
{
    class FetchInit : IFetchInit
    {
        [JsonProperty("method")]
        public string? Method { get; set; }
        [JsonProperty("Referrer")]
        public string? Referrer { get; set; }
        [JsonIgnore] public IDictionary<string, string>? Headers => HeadersInternal;
        [JsonProperty("headers")]
        public Dictionary<string, string> HeadersInternal { get; set; }
        [JsonProperty("bodyType")]
        public string? BodyType { get; set; }
        [JsonIgnore] public IDictionary<string, string>? Body => BodyInternal;
        [JsonProperty("body")]
        public Dictionary<string, string> BodyInternal { get; set; }
    }
}
