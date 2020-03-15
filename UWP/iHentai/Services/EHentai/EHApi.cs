using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System.RemoteSystems;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using iHentai.Common;
using iHentai.Common.Helpers;
using iHentai.Common.Html;
using iHentai.Services.Core;
using iHentai.Services.EHentai.Model;
using Microsoft.Toolkit.Helpers;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Toolkit.Uwp.UI.Animations.Behaviors;
using ColorHelper = Microsoft.Toolkit.Uwp.Helpers.ColorHelper;

namespace iHentai.Services.EHentai
{
    internal static class ApiExtensions
    {
        public static Task<T> GetHtmlAsync<T>(
            this Url url,
            CancellationToken cancellationToken = default,
            HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
        {
            return new FlurlRequest(url).GetHtmlAsync<T>(cancellationToken, completionOption);
        }


        public static Task<T> GetHtmlAsync<T>(
            this IFlurlRequest request,
            CancellationToken cancellationToken = default,
            HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
        {
            return request.SendAsync(HttpMethod.Get, null, cancellationToken, completionOption).ReceiveHtml<T>();
        }

        public static async Task<T> ReceiveHtml<T>(this Task<IFlurlResponse> response)
        {
            using var resp = await response.ConfigureAwait(false);
            if (resp == null)
            {
                return default;
            }

            var result = await resp.GetStringAsync().ConfigureAwait(false);
            return HtmlConvert.DeserializeObject<T>(result);
        }
    }

    internal class ExApi : EHApi, ICustomHttpHandler
    {
        private const string COOKIE_KEY = "exhentai_cookie";
        protected override string Host => "https://exhentai.org/";
        public bool RequireLogin => string.IsNullOrEmpty(GetCookie());

        public ExApi()
        {
            Singleton<HentaiHttpMessageHandler>.Instance.RegisterHandler(this);
        }

        public bool CanHandle(Uri uri)
        {
            return uri.Host.Equals("exhentai.org", StringComparison.InvariantCultureIgnoreCase);
        }

        public void Handle(HttpRequestMessage message)
        {
            message.Headers.Add("Cookie", GetCookie() + ";igneous=");
        }

        private string GetCookie()
        {
            return Singleton<Settings>.Instance.Get(COOKIE_KEY, string.Empty);
        }
        

        private void SetCookie(string cookie)
        {
            Singleton<Settings>.Instance.Set(COOKIE_KEY, cookie.TrimEnd(';'));
        }
        
        public async Task Login(string userName, string password)
        {
            using var handler = new NoCookieHttpMessageHandler();
            using var client = new HttpClient(handler);
            using var request = new HttpRequestMessage(HttpMethod.Post,
                "http://forums.e-hentai.org/index.php?act=Login&CODE=01&CookieDate=1")
            {
                Content = new FormUrlEncodedContent(new[]
                {
                    KeyValuePair.Create("UserName", userName), KeyValuePair.Create("PassWord", password)
                })
            };
            using var response = await client.SendAsync(request, completionOption: HttpCompletionOption.ResponseHeadersRead);
            response.Headers.TryGetValues("Set-Cookie", out var cookies);
            var cookie = cookies
                .Select(item => item.Split(';').FirstOrDefault())
                .Let(it => string.Join(";", it));
            //cookie = await UpdateCookie(cookie);
            SetCookie(cookie);
        }

        private async Task<string> UpdateCookie(string cookie)
        {
            using var handler = new NoCookieHttpMessageHandler();
            using var client = new HttpClient(handler);
            using var request = new HttpRequestMessage(HttpMethod.Get, "http://forums.e-hentai.org/index.php?act=Login&CODE=01&CookieDate=1");
            request.Headers.Add("Cookie", $"{cookie};uconfig=");
            using var response = await client.SendAsync(request);
            response.Headers.TryGetValues("Set-Cookie", out var cookies);
            return cookies
                .Select(item => item.Split(';').FirstOrDefault())
                .Let(it => string.Join(";", it));
        }
    }

    internal class EHApi
    {
        protected virtual string Host => "https://e-hentai.org/";

        public EHApi()
        {
            Singleton<HtmlCache<EHGalleryImage>>.Instance.CacheDuration = TimeSpan.FromDays(7);
        }

        public async Task<IEnumerable<IGallery>> Home(int page = 0)
        {
            var result = await $"{Host}"
                .SetQueryParams(new
                {
                    page
                })
                .GetHtmlAsync<EHGalleryList>();
            return result.Items;
        }

        public async Task<EHGalleryDetail> Detail(string link, bool removeCache = false)
        {
            if (removeCache)
            {
                await Singleton<HtmlCache<EHGalleryDetail>>.Instance.RemoveAsync(new[] {new Uri(link)});
            }

            return await Singleton<HtmlCache<EHGalleryDetail>>.Instance.GetFromCacheAsync(new Uri(link));
        }

        public async Task<EHGalleryImage> GetImage(string link, bool removeCache = false, CancellationToken token = default)
        {
            if (removeCache)
            {
                await Singleton<HtmlCache<EHGalleryImage>>.Instance.RemoveAsync(new[] {new Uri(link)});
            }
            return await Singleton<HtmlCache<EHGalleryImage>>.Instance.GetFromCacheAsync(new Uri(link), cancellationToken: token);
        }

        public static string GetLanguageTag(string title)
        {
            var result = string.Empty;
            ContainText(title, "english", "EN", ref result);
            ContainText(title, "chinese", "ZH", ref result);
            ContainText(title, "dutch", "NL", ref result);
            ContainText(title, "french", "FR", ref result);
            ContainText(title, "german", "DE", ref result);
            ContainText(title, "hungarian", "HU", ref result);
            ContainText(title, "italian", "IT", ref result);
            ContainText(title, "korean", "KO", ref result);
            ContainText(title, "polish", "PL", ref result);
            ContainText(title, "portuguese", "PT", ref result);
            ContainText(title, "russian", "RU", ref result);
            ContainText(title, "spanish", "ES", ref result);
            ContainText(title, "thai", "TH", ref result);
            ContainText(title, "vietnamese", "VI", ref result);
            return result;
        }

        public static SolidColorBrush GetCatColorBrush(EHCategory category)
        {
            return new SolidColorBrush(category switch
            {
                EHCategory.Doujinshi => ColorHelper.ToColor("#f26f5f"),
                EHCategory.Manga => ColorHelper.ToColor("#fcb417"),
                EHCategory.ArtistCG => ColorHelper.ToColor("#dde500"),
                EHCategory.GameCG => ColorHelper.ToColor("#05bf0b"),
                EHCategory.Western => ColorHelper.ToColor("#14e723"),
                EHCategory.NonH => ColorHelper.ToColor("#08d7e2"),
                EHCategory.ImageSet => ColorHelper.ToColor("#5f5fff"),
                EHCategory.Cosplay => ColorHelper.ToColor("#9755f5"),
                EHCategory.AsianPorn => ColorHelper.ToColor("#fe93ff"),
                EHCategory.Misc => ColorHelper.ToColor("#9e9e9e"),
                _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
            });
        }

        private static void ContainText(string oriText, string contains, string value, ref string result)
        {
            if (oriText.Contains(contains, StringComparison.OrdinalIgnoreCase))
            {
                result = value;
            }
        }
    }
}