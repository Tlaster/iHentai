using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using iHentai.Apis.Core;
using iHentai.Apis.Core.Common;
using iHentai.Apis.Core.Models.Interfaces;
using iHentai.Apis.NHentai.Models;
using iHentai.Extensions;

namespace iHentai.Apis.NHentai
{
    public class Apis : IHentaiApis
    {
        public bool FouceLogin { get; } = false;
        public bool HasLogin { get; } = false;
        public bool CanLogin { get; } = false;
        public bool CanLoginWithWebView { get; } = false;
        public string LoginWebViewUrl { get; }
        public Dictionary<string, string> ImageRequestHeader { get; }
        public string Host { get; } = "nhentai.net";
        public IApiConfig ApiConfig { get; }
        public SearchOptionBase SearchOptionGenerator => new SearchOption();

        public bool WebViewLoginHandler(string url, string cookie)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> Cookie => throw new NotImplementedException();

        public async Task<(int MaxPage, IEnumerable<IGalleryModel> Gallery)> Gallery(int page = 0,
            SearchOptionBase searchOption = null)
        {
            var req = $"https://{Host}/".AppendPathSegment("api").AppendPathSegment("galleries");
            if (searchOption == null || searchOption.Keyword.IsEmpty())
                req.AppendPathSegment("all");
            else
                switch (searchOption.SearchType)
                {
                    case SearchTypes.Keyword:
                        req.AppendPathSegment("search").SetQueryParams(searchOption.ToDictionary());
                        break;
                    case SearchTypes.Tag:
                        req.AppendPathSegment("tagged").SetQueryParam("tag_id", searchOption.Keyword)
                            .SetQueryParam("sort",
                                (searchOption as SearchOption)?.SortEnabled == true ? "popular" : null);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            var res = await req.SetQueryParam(nameof(page), page + 1)
                .GetJsonAsync<GalleryListModel>();
            return (res.NumPages, res.Gallery.WithoutShit());
        }

        public Task<bool> Login(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public Task<IGalleryDetailModel> Detail(IGalleryModel model)
        {
            return Task.FromResult(model as IGalleryDetailModel);
        }
    }
}