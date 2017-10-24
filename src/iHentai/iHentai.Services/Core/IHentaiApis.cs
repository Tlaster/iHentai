﻿using System.Threading.Tasks;
using iHentai.Services.Core.Models.Interfaces;
using System.Collections.Generic;
using iHentai.Services.Core.Common;

namespace iHentai.Services.Core
{
    public interface IHentaiApis
    {
        bool FouceLogin { get; }
        bool HasLogin { get; }
        bool CanLogin { get; }
        Dictionary<string, string> Cookie { get; }
        string Host { get; }
        IApiConfig ApiConfig { get; }
        ISettings Settings { get; }
        SearchOptionBase GenerateSearchOptionBase { get; }
        Task<IEnumerable<IGalleryModel>> Gallery(int page = 0, SearchOptionBase searchOption = null);
        Task<IEnumerable<IGalleryModel>> TaggedGallery(string name, int page = 0);
        Task<(bool State, string Message)> Login(string userName, string password);
    }
}