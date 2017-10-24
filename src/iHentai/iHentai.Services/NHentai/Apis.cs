using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using iHentai.Services.Core;
using iHentai.Services.Core.Models.Interfaces;
using iHentai.Services.NHentai.Models;
using iHentai.Shared.Extensions;

namespace iHentai.Services.NHentai
{
    public class Apis : IHentaiApis
    {
        public bool FouceLogin { get; } = false;
        public bool HasLogin { get; } = true;
        public bool CanLogin { get; } = false;
        public string Host { get; } = "https://nhentai.net/";
        public IApiConfig ApiConfig { get; }
        public ISettings Settings { get; }
        public SearchOptionBase GenerateSearchOptionBase => new SearchOption();

        public Dictionary<string, string> Cookie => throw new NotImplementedException();

        public async Task<IEnumerable<IGalleryModel>> Gallery(int page = 0, SearchOptionBase searchOption = null)
        {
            if (searchOption == null)
                return (await $"{Host}api/galleries/all".SetQueryParam(nameof(page), page + 1)
                    .GetJsonAsync<GalleryListModel>()).Gallery;
            return (await $"{Host}api/galleries/search".SetQueryParam(nameof(page), page + 1)
                .SetQueryParams(searchOption.ToDictionary())
                .GetJsonAsync<GalleryListModel>()).Gallery;
//            return (await Host.SetQueryParam("page", page + 1).GetHtmlAsync<GalleryListModel>()).Gallery;
        }

        public Task<(bool State, string Message)> Login(string userName, string password)
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<IGalleryModel>> TaggedGallery(string tag_id, int page = 0)
        {
            return TaggedGallery(tag_id, page, false);
        }

        public async Task<IEnumerable<IGalleryModel>> TaggedGallery(string tag_id, int page = 0, bool sort = false)
        {
            return (await $"{Host}api/galleries/tagged".SetQueryParam(nameof(page), page + 1)
                .SetQueryParam(nameof(tag_id), tag_id)
                .SetQueryParam(nameof(sort), sort ? "popular" : null)
                .GetJsonAsync<GalleryListModel>()).Gallery;
        }
    }
}