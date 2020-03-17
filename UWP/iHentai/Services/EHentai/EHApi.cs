using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System.RemoteSystems;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Flurl;
using Flurl.Http.Configuration;
using iHentai.Common;
using iHentai.Common.Helpers;
using iHentai.Services.Core;
using iHentai.Services.EHentai.Model;
using Microsoft.Toolkit.Helpers;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Toolkit.Uwp.UI.Animations.Behaviors;
using ColorHelper = Microsoft.Toolkit.Uwp.Helpers.ColorHelper;

namespace iHentai.Services.EHentai
{
    internal class EHApi
    {
        public virtual string Host => "https://e-hentai.org/";

        public EHApi()
        {
            Singleton<HtmlCache<EHGalleryImage>>.Instance.CacheDuration = TimeSpan.FromDays(7);
        }

        public async Task<IEnumerable<EHGallery>> Tag(string link, int page = 0, int from = 0)
        {
            var result = await $"{link}{(page == 0 ? "" : $"/{page}")}".Let(it =>
                {
                    if (from != 0)
                    {
                        return it.SetQueryParams(new
                        {
                            from
                        });
                    }

                    return new Url(it);
                })
                .GetHtmlAsync<EHGalleryList>();
            return result.Items.Where(it => !string.IsNullOrEmpty(it.Link));
        }

        public async Task<IEnumerable<EHGallery>> Gallery(string link, int page = 0)
        {
            var result = await $"{link}".Let(it =>
                {
                    if (page != 0)
                    {
                        return it.SetQueryParams(new
                        {
                            page
                        });
                    }

                    return new Url(it);
                })
                .GetHtmlAsync<EHGalleryList>();
            return result.Items.Where(it => !string.IsNullOrEmpty(it.Link));
        }

        public Task<IEnumerable<EHGallery>> Home(int page = 0)
        {
            return Gallery(Host, page);
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
            if (oriText?.Contains(contains, StringComparison.OrdinalIgnoreCase) == true)
            {
                result = value;
            }
        }

        public List<string> GetSearchSuggestion(string queryText)
        {
            return Singleton<Settings>.Instance.Get(GetType().Name + "_search_list", new List<string>()).Where(it => it.Contains(queryText)).ToList();
        }

        public void SetSearchSuggestion(string queryText)
        {
            if (string.IsNullOrEmpty(queryText))
            {
                return;
            }
            var current = Singleton<Settings>.Instance.Get(GetType().Name + "_search_list", new List<string>());
            current.Remove(queryText);
            current.Insert(0, queryText);
            Singleton<Settings>.Instance.Set(GetType().Name + "_search_list", current);
        }

        public Task<IEnumerable<EHGallery>> Search(SearchOption searchOption, int page = 0)
        {
            return Gallery(Host + "?" + searchOption.ToSearchParameter(), page);
        }
    }
}