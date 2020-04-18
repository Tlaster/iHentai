using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using iHentai.Services.Core;
using iHentai.Services.EHentai.Model;

namespace iHentai.Services.EHentai
{
    public class EHApi : IHentaiApi
    {
        protected EHApi()
        {
        }

        public static EHApi Instance { get; } = new EHApi();
        public virtual string Host => "https://e-hentai.org/";
        public virtual string Name { get; } = "ehentai";

        public async Task<IEnumerable<EHGallery>> Tag(string link, int page = 0, int from = 0)
        {
            var url = $"{link}{(page == 0 ? "" : $"/{page}")}";
            var request = true switch
            {
                _ when from != 0 => url.SetQueryParams(new
                {
                    from
                }),
                _ => new Url(url)
            };

            var result = await request.GetHtmlAsync<EHGalleryList>();
            return result.Items.Where(it => !string.IsNullOrEmpty(it.Link));
        }

        public async Task<IEnumerable<EHGallery>> Gallery(string link, int page = 0)
        {
            var url = $"{link}";
            var request = true switch
            {
                _ when page != 0 => url.SetQueryParams(new
                {
                    page
                }),
                _ => new Url(url)
            };
            var result = await request.GetHtmlAsync<EHGalleryList>();
            return result.Items.Where(it => !string.IsNullOrEmpty(it.Link));
        }

        public Task<IEnumerable<EHGallery>> Home(int page = 0)
        {
            return Gallery(Host, page);
        }

        public async Task<EHGalleryDetail> Detail(string link)
        {
            return await new Url(link).GetHtmlAsync<EHGalleryDetail>();
        }

        public async Task<EHGalleryImage> GetImage(string link, string? nl = null, CancellationToken token = default)
        {
            var request = true switch
            {
                _ when nl == null => new Url(link),
                _ => link.SetQueryParam("nl", nl)
            };
            return await request.GetHtmlAsync<EHGalleryImage>(token);
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

        public static string GetCatColorBrush(EHCategory category)
        {
            return category switch
            {
                EHCategory.Doujinshi => "#f26f5f",
                EHCategory.Manga => "#fcb417",
                EHCategory.ArtistCG => "#dde500",
                EHCategory.GameCG => "#05bf0b",
                EHCategory.Western => "#14e723",
                EHCategory.NonH => "#08d7e2",
                EHCategory.ImageSet => "#5f5fff",
                EHCategory.Cosplay => "#9755f5",
                EHCategory.AsianPorn => "#fe93ff",
                EHCategory.Misc => "#9e9e9e",
                _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
            };
        }

        private static void ContainText(string oriText, string contains, string value, ref string result)
        {
            if (oriText?.ToLower()?.Contains(contains?.ToLower()) == true)
            {
                result = value;
            }
        }
        
        public Task<IEnumerable<EHGallery>> Search(SearchOption searchOption, int page = 0)
        {
            return Gallery(Host + "?" + searchOption.ToSearchParameter(), page);
        }
    }
}