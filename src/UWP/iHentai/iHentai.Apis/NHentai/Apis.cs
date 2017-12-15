using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using iHentai.Apis.Core;
using iHentai.Apis.Core.Common;
using iHentai.Apis.Core.Models.Interfaces;
using iHentai.Apis.NHentai.Models;
using iHentai.Basic.Extensions;

namespace iHentai.Apis.NHentai
{
    public class Apis : IHentaiApi, IWebApi
    {

        public string Host { get; } = "nhentai.net";
        
        public SearchOptionBase SearchOptionGenerator => new SearchOption();
        
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

        public Task<IGalleryDetailModel> Detail(IGalleryModel model)
        {
            return Task.FromResult(model as IGalleryDetailModel);
        }

        public string GetWebLink(IGalleryModel model)
        {
            if (!(model is GalleryModel item))
            {
                return string.Empty;
            }
            return $"https://{Host}/".AppendPathSegment("g").AppendPathSegment(item.Id);
        }
    }
}