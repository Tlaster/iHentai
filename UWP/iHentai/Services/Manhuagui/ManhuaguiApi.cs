using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using iHentai.Common;
using iHentai.Common.Helpers;
using iHentai.Services.EHentai;
using iHentai.Services.Manhuagui.Model;
using LZStringCSharp;
using Microsoft.Toolkit.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iHentai.Services.Manhuagui
{
    class ManhuaguiApi : ICustomHttpHandler
    {
        public virtual string Host => "https://m.manhuagui.com";

        public ManhuaguiApi()
        {
            Singleton<HentaiHttpMessageHandler>.Instance.RegisterHandler(this);
        }

        public async Task<List<ManhuaguiGallery>> Update(int page = 1)
        {
            return await $"{Host}/update/".SetQueryParams(new
            {
                page,
                ajax = 1,
                order = 1
            }).GetHtmlAsync<List<ManhuaguiGallery>>();
        }

        public async Task<ManhuaguiGalleryDetail> Detail(string path)
        {
            return await new Url($"{Host}{path}")
                .GetHtmlAsync<ManhuaguiGalleryDetail>();
        }

        public async Task<List<string>> Images(string remotePath)
        {
            var html = await $"{Host}{remotePath}".GetStringAsync();
            var packed = Regex.Match(html, "\\(function\\(p,a,c,k,e,d\\).*?0,\\{\\}\\)\\)").Value;
            var packges = packed.Split(',');
            var replaceable = packges[packges.Length - 3];
            var base64LZString = replaceable.Split('\'')[1];
            var script = packed.Replace(replaceable, $"'{LZString.DecompressFromBase64(base64LZString)}'.split('|')");
            var result = Singleton<ScriptEngine>.Instance.RunScript(script);
            var json = result.Substring(11, result.Length - 23);
            var jobj = JsonConvert.DeserializeObject<JObject>(json);
            var chapterId = jobj.Value<string>("chapterId");
            var bookId = jobj.Value<string>("bookId");
            var md5 = jobj["sl"].Value<string>("md5");
            var files = jobj["files"] as JArray ?? jobj["images"];
            return files.Select(it => $"https://i.hamreus.com{it}?cid={chapterId}&md5={md5}&bookId=${bookId}").ToList();
        }

        public bool CanHandle(Uri uri)
        {
            return uri.Host.Equals("i.hamreus.com", StringComparison.InvariantCultureIgnoreCase);
        }

        public void Handle(HttpRequestMessage message)
        {
            var url = new Url(message.RequestUri);
            var bookId = url.QueryParams["bookId"];
            var chapterId = url.QueryParams["cid"];
            if (bookId != null && chapterId != null)
            {
                message.Headers.Referrer = new Uri($"https://m.manhuagui.com/comic/{bookId}/{chapterId}.html");
            }
        }
    }
}
