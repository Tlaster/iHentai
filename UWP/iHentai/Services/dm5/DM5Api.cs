using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using iHentai.Common;
using iHentai.Common.Helpers;
using iHentai.Services.Core;
using iHentai.Services.dm5.Model;
using iHentai.Services.EHentai;
using LZStringCSharp;
using Microsoft.Toolkit.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iHentai.Services.dm5
{
    class DM5Api : IMangaApi, ICustomHttpHandler
    {
        enum Status
        {
            All = 0,
            Living = 1,
            Completed = 2,
        }

        enum Sort
        {
            Hot = 10,
            LatestUpdate = 2,
            LatestAdded = 18
        }

        private const string Host = "https://m.dm5.com";
        public string Name { get; } = "DM5";

        public DM5Api()
        {
            Singleton<HentaiHttpMessageHandler>.Instance.RegisterHandler(this);
        }

        public async Task<IEnumerable<IMangaGallery>> Home(int page)
        {
            return await Update(page + 1);
        }

        public async Task<IEnumerable<IMangaGallery>> Search(string keyword, int page)
        {
            var result = await $"{Host}/pagerdata.ashx".PostUrlEncodedAsync(new
            {
                t = 7,
                pageindex = page + 1,
                f = 0,
                title = keyword,
            }).ReceiveJson<List<DM5SearchGallery>>();
            return result;
        }

        public async Task<IMangaDetail> Detail(IMangaGallery gallery)
        {
            var urlKey = gallery switch
            {
                DM5Gallery item => item.UrlKey,
                DM5SearchGallery searchItem => searchItem.Url.TrimEnd('/').TrimEnd('/'),
                _ => throw new ArgumentException()
            };
            return await new Url($"{Host}/{urlKey}").GetHtmlAsync<DM5GalleryDetail>();
        }

        public Task<List<string>> ChapterImages(IMangaChapter chapter)
        {
            if (chapter is DM5GalleryChapter item)
            {
                return Images(item.Link);
            }

            throw new ArgumentException();
        }

        public async Task<List<DM5Gallery>> Update(int page = 1)
        {
            var result = await $"{Host}/dm5.ashx".PostUrlEncodedAsync(new
            {
                action = "getclasscomics",
                pageindex = page,
                pagesize = 20,
                categoryid = 0,
                tagid = 0,
                status = (int) Status.All,
                usergroup = 0,
                pay = -1,
                areaid = 0,
                sort = (int) Sort.LatestUpdate,
                iscopyright = 0,
            }).ReceiveJson<Dm5Response>();

            return result.UpdateComicItems;
        }

        
        public async Task<List<string>> Images(string remotePath)
        {
            var html = await $"{Host}{remotePath}".GetStringAsync();
            var packed = Regex.Match(html, "\\(function\\(p,a,c,k,e,d\\).*?0,\\{\\}\\)\\)").Value;
            var scriptResult = Singleton<ScriptEngine>.Instance.RunScript(packed);
            var newIms = scriptResult.Substring(scriptResult.IndexOf('[')).TrimEnd(';');
            var items = JsonConvert.DeserializeObject<List<string>>(newIms);
            return items.Select(it => it + "&bookId=" + remotePath.TrimStart('/')).ToList();
        }

        public bool CanHandle(Uri uri)
        {
            return uri.Host.Contains("dm5.com", StringComparison.InvariantCultureIgnoreCase);
        }

        public void Handle(HttpRequestMessage message)
        {
            var url = new Url(message.RequestUri);
            var bookId = url.QueryParams["bookId"];
            if (bookId != null)
            {
                message.Headers.Referrer = new Uri($"https://m.dm5.com/{bookId}/");
            }
        }
    }
}
