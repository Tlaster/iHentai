using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Flurl;
using Flurl.Http;
using iHentai.Common.Html;
using iHentai.Services.Core;
using iHentai.Services.EHentai.Model;
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


    internal class EHApi
    {
        private const string HOST = "https://e-hentai.org/";

        public async Task<IEnumerable<IGallery>> Home(int page = 0)
        {
            var result = await $"{HOST}"
                .SetQueryParams(new
                {
                    page
                })
                .GetHtmlAsync<EHGalleryList>();
            return result.Items.Where(it => it.Title != null);
        }

        public async Task<EHGalleryDetail> Detail(string link, int page = 0)
        {
            return await link.SetQueryParams(new
                {
                    p = page
                })
                .GetHtmlAsync<EHGalleryDetail>();
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