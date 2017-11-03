using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using iHentai.Services.Core;
using iHentai.Services.Core.Common;
using iHentai.Services.Core.Models.Interfaces;
using iHentai.Services.NHentai.Models;

namespace iHentai.Services.NHentai
{
    public class Apis : IHentaiApis
    {
        public bool FouceLogin { get; } = false;
        public bool HasLogin { get; } = true;
        public bool CanLogin { get; } = false;
        public string Host { get; } = "https://nhentai.net/";
        public IApiConfig ApiConfig { get; }
        public ISettings Settings { get; } = new Settings("nhentai");
        public SearchOptionBase GenerateSearchOptionBase => new SearchOption();

        public Dictionary<string, string> Cookie => throw new NotImplementedException();

        public async Task<(int MaxPage, IEnumerable<IGalleryModel> Gallery)> Gallery(int page = 0,
            SearchOptionBase searchOption = null)
        {
            var req = Host.AppendPathSegment("api").AppendPathSegment("galleries");
            if (searchOption == null)
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
            return (res.NumPages, res.Gallery);
        }

        public Task<(bool State, string Message)> Login(string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}