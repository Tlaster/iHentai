using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using iHentai.Apis.Core;
using iHentai.Apis.Core.Common;
using iHentai.Apis.Core.Models.Interfaces;
using iHentai.Apis.EHentai.Models;
using iHentai.Extensions;
#if !UNIT_TEST
using iHentai.Helpers;

#endif

namespace iHentai.Apis.EHentai
{
    public class Apis : IHentaiApi, ILoginApi, ICookieApi, IConfigApi
    {
        public bool IsExhentaiMode { get; set; } = true;
        public bool FouceLogin { get; } = true;
        public bool HasLogin => Cookie?.Any() == true;
        public bool CanLoginWithWebView { get; } = true;
        public string LoginWebViewUrl { get; } = "https://forums.e-hentai.org/index.php?act=Login";
        public SearchOptionBase SearchOptionGenerator => new SearchOption();

        public Dictionary<string, string> RequestHeader => new Dictionary<string, string>
        {
            {"Cookie", string.Join(";", Cookie.Select(item => $"{item.Key}={item.Value}").Concat(new[] {"igneous=", $"uconfig={ApiConfig}"}))}
        };

        public string Host => IsExhentaiMode ? "exhentai.org" : "g.e-hentai.org";

        public IApiConfig ApiConfig
        {
            get => "exhentai_config".Read(new Config());
            set => value.Save("exhentai_config");
        }

        public Dictionary<string, string> Cookie
        {
            get => "exhentai_user_info".Read(new Dictionary<string, string>());
            set => value.Save("exhentai_user_info");
        }

        public bool WebViewLoginHandler(string url, string cookie)
        {
            if (!cookie.Contains("ipb_member_id") || !cookie.Contains("ipb_pass_hash")) return false;
            var memberid = Regex.Match(cookie, @"ipb_member_id=([^;]*)").Groups[1].Value;
            var passHash = Regex.Match(cookie, @"ipb_pass_hash=([^;]*)").Groups[1].Value;
            //TODO:Check with "s=" cookie
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
                req = $"https://{Host}/"
                    .AppendPathSegment("tag")
                    .AppendPathSegment(searchOption.Keyword);
            else
                req = $"https://{Host}/".SetQueryParams(searchOption?.ToDictionary());
            var res = await req.SetQueryParam("page", page)
                .GetHtmlAsync<GalleryListModel>();
            return (res.MaxPage, res.Gallery.WithoutShit());
        }

        public async Task<bool> Login(string userName, string password)
        {
            Dictionary<string, string> cookie = null;
            using (var loginClient = new FlurlClient { Settings = { HttpClientFactory = new DefaultHttpClientFactory() } })
            {
                using (var res = await "http://forums.e-hentai.org/index.php?act=Login&CODE=01&CookieDate=1".WithClient(loginClient).EnableCookies()
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
                            (Key: Regex.Matches(item, "([^=]*)=([^;]*);")[0].Groups[1].Value, Regex.Matches(item,
                                "([^=]*)=([^;]*);")[0].Groups[2].Value))
                        .Distinct(item => item.Key)
                        .Where(item => item.Key == "ipb_member_id" || item.Key == "ipb_pass_hash")
                        .ToDictionary(item => item.Key, item => item.Value);
                }
                if (!cookie.Any())
                    return false;
            }
            using (var loginClient = new FlurlClient { Settings = { HttpClientFactory = new DefaultHttpClientFactory() } })
            {
                using (var res = await "https://exhentai.org/uconfig.php".WithClient(loginClient).WithCookies(cookie)
                    .WithCookie("uconfig", ApiConfig.ToString()).GetAsync())
                {
                    if (res.Headers.TryGetValues("Set-Cookie", out var cookies) &&
                        cookies.Any(item => item.StartsWith("s=")))
                    {
                        cookie = cookies
                            .Select(item =>
                                (Key: Regex.Matches(item, "([^=]*)=([^;]*);")[0].Groups[1].Value, Regex.Matches(item,
                                    "([^=]*)=([^;]*);")[0].Groups[2].Value))
                            .Distinct(item => item.Key)
                            .Where(item => item.Key == "s")
                            .ToDictionary(item => item.Key, item => item.Value)
                            .Concat(cookie)
                            .ToDictionary(item => item.Key, item => item.Value);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            Cookie = cookie;
            return true;
        }

        public async Task<IGalleryDetailModel> Detail(IGalleryModel model)
        {
            if (!(model is GalleryModel item))
            {
                throw new ArgumentException();
            }
            return await $"https://{Host}/"
                .AppendPathSegment("g")
                .AppendPathSegment(item.ID)
                .AppendPathSegment(item.Token)
                .GetHtmlAsync<GalleryDetailModel>();
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