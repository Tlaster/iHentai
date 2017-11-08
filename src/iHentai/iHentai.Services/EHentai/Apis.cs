using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using iHentai.Services.Core;
using iHentai.Services.Core.Common;
using iHentai.Services.Core.Models.Interfaces;
using iHentai.Services.EHentai.Models;

namespace iHentai.Services.EHentai
{
    public class Apis : IHentaiApis
    {
        public bool IsExhentaiMode { get; set; }
        public bool FouceLogin { get; } = true;
        public bool HasLogin => Cookie?.Any() == true;
        public bool CanLogin { get; } = true;
        public bool CanLoginWithWebView { get; } = true;
        public string LoginWebViewUrl { get; } = "http://forums.e-hentai.org/index.php?act=Login";
        public string Host => IsExhentaiMode ? "http://g.e-hentai.org/" : "https://exhentai.org/";

        public IApiConfig ApiConfig
        {
#if DEBUG
            
            get;
            set;
#else
            get => Settings.Get("ehentai_config", new Config());
            set => Settings.Set("ehentai_config", value);
#endif
        }
#if DEBUG
        = new Config();
#endif

        public ISettings Settings { get; } = new Settings("ehentai");
        public SearchOptionBase GenerateSearchOptionBase => new SearchOption();

        public bool WebViewLoginHandler(string url, string cookie)
        {
            if (!cookie.Contains("ipb_member_id") || !cookie.Contains("ipb_pass_hash")) return false;
            var memberid = Regex.Match(cookie, @"ipb_member_id=([^;]*)").Groups[1].Value;
            var passHash = Regex.Match(cookie, @"ipb_pass_hash=([^;]*)").Groups[1].Value;

            Cookie = new Dictionary<string, string>
            {
                {"ipb_member_id", memberid},
                {"ipb_pass_hash", passHash}
            };
            return true;
        }

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

        public async Task<(int MaxPage, IEnumerable<IGalleryModel> Gallery)> Gallery(int page = 0,
            SearchOptionBase searchOption = null)
        {
            Url req;
            if (searchOption != null && searchOption.SearchType == SearchTypes.Tag)
                req = $"{Host}{searchOption.Keyword}"
                    .AppendPathSegment("tag");
            else
                req = Host.SetQueryParams(searchOption?.ToDictionary());
            var res = await req.SetQueryParam("page", page).WithCookies(Cookie)
                .WithCookie("uconfig", ApiConfig.ToString()).GetHtmlAsync<GalleryListModel>();
            return (res.MaxPage, res.Gallery);
        }

        public void LoginWithMenberId(string ipb_member_id, string ipb_pass_hash)
        {
            Cookie = new Dictionary<string, string>
            {
                {nameof(ipb_member_id), ipb_member_id},
                {nameof(ipb_pass_hash), ipb_pass_hash}
            };
        }
        
        public async Task<(bool State, string Message)> Login(string userName, string password)
        {
            Dictionary<string, string> cookie = null;
            using (var res = await "http://forums.e-hentai.org/index.php?act=Login&CODE=01&CookieDate=1".EnableCookies()
                .PostUrlEncodedAsync(new
                {
                    UserName = userName,
                    PassWord = password,
                    x = 0,
                    y = 0
                }))
            {
                res.Headers.TryGetValues("Set-Cookie", out var cookies);
                cookie = cookies
                    .Select(item =>
                        (Key: Regex.Matches(item, "([^=]*)=([^;]*);")[0].Groups[1].Value, Value: Regex.Matches(item,
                            "([^=]*)=([^;]*);")[0].Groups[2].Value))
                    .Distinct(item => item.Key)
                    .ToDictionary(item => item.Key, item => item.Value);
            }
            if (!cookie.Any())
            {
                return (false, "Require more informations");
            }
            using (var res = await "https://exhentai.org/".WithCookies(cookie)
                .WithCookie("uconfig", ApiConfig.ToString()).GetAsync())
            {
                if (res.Headers.Contains("ContentType") &&
                    res.Headers.GetValues("ContentType")?.FirstOrDefault() == "image/gif")
                    return (false, "No access to exhentai.org");
            }
            Cookie = cookie;
            return (true, string.Empty);
        }
    }
}