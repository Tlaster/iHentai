using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl.Http;
using iHentai.Services.Core;
using iHentai.Services.Core.Models.Interfaces;
using Flurl.Util;
using System.Linq;
using System.Text.RegularExpressions;
using iHentai.Services.EHentai.Models;
using Html2Model;
using Flurl;
using iHentai.Services.Core.Common;

namespace iHentai.Services.EHentai
{
    public class Apis : IHentaiApis
    {
        public bool IsExhentaiMode { get; set; }
        public bool FouceLogin { get; } = true;
        public bool HasLogin => Cookie.Any();
        public bool CanLogin { get; } = true;
        public string Host => IsExhentaiMode ? "http://g.e-hentai.org/" : "https://exhentai.org/";
        public IApiConfig ApiConfig { get; } = new Config();
        public ISettings Settings { get; } = new Settings("EHentai");
        public SearchOptionBase GenerateSearchOptionBase => new SearchOption();

        public Dictionary<string, string> Cookie
        {
#if DEBUG
            get;
            set;
#else
            get => Settings.Get<string>("user_info").FromJson<Dictionary<string, string>>();
            set => Settings.Set("user_info", value.ToJson());
#endif
        }

        public async Task<IEnumerable<IGalleryModel>> Gallery(int page = 0, SearchOptionBase searchOption = null)
        {
            return (await Host.SetQueryParam("page", page).SetQueryParams(searchOption?.ToDictionary()).WithCookies(Cookie).WithCookie("uconfig", ApiConfig.ToString()).GetHtmlAsync<GalleryListModel>()).Gallery;
        }

        public async Task<(bool State, string Message)> Login(string userName, string password)
        {
            Dictionary<string, string> cookie = null;
            using (var res = (await "http://forums.e-hentai.org/index.php?act=Login&CODE=01&CookieDate=1".EnableCookies()
                                             .PostUrlEncodedAsync(new
                                             {
                                                 UserName = userName,
                                                 PassWord = password,
                                                 x = 0,
                                                 y = 0,
                                             })))
            {
                res.Headers.TryGetValues("Set-Cookie", out var cookies);
                cookie = cookies
                    .Select(item => (Key: Regex.Matches(item, "([^=]*)=([^;]*);")[0].Groups[1].Value, Value: Regex.Matches(item, "([^=]*)=([^;]*);")[0].Groups[2].Value))
                    .Distinct(item => item.Key)
                    .ToDictionary(item => item.Key, item => item.Value);
            }
            using (var res = await "https://exhentai.org/".WithCookies(cookie).WithCookie("uconfig", ApiConfig.ToString()).GetAsync())
            {
                if (res.Headers.Contains("ContentType") && res.Headers.GetValues("ContentType")?.FirstOrDefault() == "image/gif")
                {
                    return (false, "No access to exhentai.org");
                }
            }
            Cookie = cookie;
            return (true, string.Empty);
        }

        public Task<IEnumerable<IGalleryModel>> TaggedGallery(string name, int page = 0)
        {
            throw new NotImplementedException();
        }
    }
}