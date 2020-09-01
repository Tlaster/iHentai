using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using iHentai.Services.Core;
using iHentai.Services.EHentai;
using iHentai.Services.Manhuagui.Model;
using LZStringCSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iHentai.Services.Manhuagui
{
    /// order: 0: Add time
    ///        1: Update time
    ///        2: View count

    class ManhuaguiApi : IMangaApi, ICustomHttpHandler
    {
        private string Host => "https://m.manhuagui.com";
        public string Name { get; } = "Manhuagui";

        public ManhuaguiApi()
        {
            HentaiHttpMessageHandler.Instance.RegisterHandler(this);
        }

        private async Task<List<ManhuaguiGallery>> Update(int page = 1)
        {
            return await $"{Host}/update/".SetQueryParams(new
            {
                page,
                ajax = 1,
                order = 1
            }).GetHtmlAsync<List<ManhuaguiGallery>>();
        }

        public async Task<IEnumerable<IMangaGallery>> Home(int page)
        {
            return await Update(page + 1);
        }

        async Task<IEnumerable<IMangaGallery>> IMangaApi.Search(string keyword, int page)
        {
            return await Search(keyword, page + 1);
        }

        public async Task<IMangaDetail> Detail(IMangaGallery gallery)
        {
            if (gallery is ManhuaguiGallery item)
            {
                return await Detail(item.Link);
            }

            throw new ArgumentException();
        }

        public Task<List<string>> ChapterImages(IMangaChapter chapter)
        {
            if (chapter is ManhuaguGalleryChapter item)
            {
                return Images(item.Link);
            }

            throw new ArgumentException();
        }

        public bool CheckCanOpenChapter(IMangaChapter chapter)
        {
            return true;
        }

        public string GetGalleryLink(IMangaGallery gallery)
        {
            if (gallery is ManhuaguiGallery item)
            {
                return $"{Host}{item.Link}";
            }

            throw new ArgumentException();
        }

        private async Task<List<ManhuaguiGallery>> SearchFirstPage(string keyword)
        {
            var result = await new Url($"{Host}/s/{keyword}.html").GetHtmlAsync<ManhuaguiGalleryList>();
            return result.List;
        }

        private async Task<List<ManhuaguiGallery>> Search(string keyword, int page = 1)
        {
            if (page == 1)
            {
                return await SearchFirstPage(keyword);
            }
            return await $"{Host}/s/{keyword}.html".PostUrlEncodedAsync(new
            {
                page,
                ajax = 1,
                order = 0,
                key = keyword
            }).ReceiveHtml<List<ManhuaguiGallery>>();
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
            var result = ScriptEngine.Instance.RunScript(script);
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
            return uri.Host.Contains("manhuagui.com") || uri.Host.Equals("i.hamreus.com", StringComparison.InvariantCultureIgnoreCase);
        }

        public void Handle(HttpRequestMessage message)
        {
            if (message.RequestUri.Host.Equals("i.hamreus.com", StringComparison.InvariantCultureIgnoreCase))
            {
                var url = new Url(message.RequestUri);
                var bookId = url.QueryParams["bookId"];
                var chapterId = url.QueryParams["cid"];
                if (bookId != null && chapterId != null)
                {
                    message.Headers.Referrer = new Uri($"{Host}/comic/{bookId}/{chapterId}.html");
                }   
            }
        }
    }
}
