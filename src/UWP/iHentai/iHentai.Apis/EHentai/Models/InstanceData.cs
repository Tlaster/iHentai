using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using iHentai.Apis.Core;
using iHentai.Services;

namespace iHentai.Apis.EHentai.Models
{
    public class InstanceData : IInstanceData, IHttpHandler
    {
        public Dictionary<string, string> Cookies { get; set; }

        public Config ApiConfig { get; set; } = new Config();

        public bool Handle(ref HttpRequestMessage message)
        {
            if (message.RequestUri.Host != nameof(EHentai).Get<IHentaiApi>().Host) return false;

            message.Headers.Add("Cookie", string.Join(";",
                Cookies.Select(item => $"{item.Key}={item.Value}")
                    .Concat(new[] {"igneous=", $"uconfig={ApiConfig}"})));
            return true;
        }
    }
}