using System.Collections.Generic;
using System.Threading.Tasks;
using iHentai.Services.Core.Common;
using iHentai.Services.Core.Models.Interfaces;

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
        Task<(int MaxPage, IEnumerable<IGalleryModel> Gallery)> Gallery(int page = 0, SearchOptionBase searchOption = null);
        Task<(bool State, string Message)> Login(string userName, string password);
    }
}