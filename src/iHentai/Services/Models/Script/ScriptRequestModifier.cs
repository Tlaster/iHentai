using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace iHentai.Services.Models.Script
{
    public class ScriptRequestContent
    {
        [JsonProperty("header")]
        public Dictionary<string, string> Header { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        internal static ScriptRequestContent FromHttpRequestMessage(HttpRequestMessage message)
        {
            return new ScriptRequestContent
            {
                Uri = message.RequestUri.ToString(),
                Header = message.Headers.ToDictionary(x => x.Key, x => string.Join(";", x.Value))
            };
        }

        internal void ToHttpRequestMessage(HttpRequestMessage message)
        {
            message.Headers.Clear();
            foreach (var item in Header)
            {
                message.Headers.Add(item.Key, item.Value);
            }
        }
    }
}