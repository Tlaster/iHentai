using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using iHentai.Apis.Core;
using iHentai.Apis.Core.Common;
using iHentai.Apis.Core.Models.Interfaces;
using iHentai.Apis.EHentai.Models;
#if !UNIT_TEST
using iHentai.Helpers;
#endif

namespace iHentai.Apis.EHentai
{
    public class Apis : IHentaiApis
    {
        public bool IsExhentaiMode { get; set; } = true;
        public bool FouceLogin { get; } = true;
        public bool HasLogin => Cookie?.Any() == true;
        public bool CanLogin { get; } = true;
        public bool CanLoginWithWebView { get; } = true;
        public string LoginWebViewUrl { get; } = "https://forums.e-hentai.org/index.php?act=Login";

        public Dictionary<string, string> ImageRequestHeader => new Dictionary<string, string>
        {
            {"Cookie", string.Join(";", Cookie.Select(item => $"{item.Key}={item.Value}").Concat(new[] {"igneous="}))}
        };

        public string Host => IsExhentaiMode ? "exhentai.org" : "g.e-hentai.org";

        public IApiConfig ApiConfig
        {
#if UNIT_TEST
            get;
            set;
#else
            get => "exhentai_config".Read(new Config());
            set => value.Save("exhentai_config");
#endif
        }
#if UNIT_TEST
        = new Config();
#endif

        public Dictionary<string, string> Cookie
        {
#if UNIT_TEST
            get;
            set;
#else 
            get => "exhentai_user_info".Read(new Dictionary<string, string>());
            set => value.Save("exhentai_user_info");
#endif
        }
#if UNIT_TEST
            = new Dictionary<string, string>();
#endif


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

        public async Task<(int MaxPage, IEnumerable<IGalleryModel> Gallery)> Gallery(int page = 0,
            SearchOptionBase searchOption = null)
        {
            Url req;
            if (searchOption != null && searchOption.SearchType == SearchTypes.Tag)
                req = $"https://{Host}/{searchOption.Keyword}"
                    .AppendPathSegment("tag");
            else
                req = $"https://{Host}/".SetQueryParams(searchOption?.ToDictionary());
            var res = await req.SetQueryParam("page", page).WithCookies(Cookie)
                .WithCookie("uconfig", ApiConfig.ToString()).GetHtmlAsync<GalleryListModel>();
            return (res.MaxPage, res.Gallery);
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
                return (false, "Require more informations");
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

        public void LoginWithMenberId(string ipb_member_id, string ipb_pass_hash)
        {
            Cookie = new Dictionary<string, string>
            {
                {nameof(ipb_member_id), ipb_member_id},
                {nameof(ipb_pass_hash), ipb_pass_hash}
            };
        }
    }
}