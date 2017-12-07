using System.Collections.Generic;
using System.Threading.Tasks;
using iHentai.Apis.Core.Models.Interfaces;

namespace iHentai.Apis.Core
{
    public interface IHentaiApis
    {
        bool FouceLogin { get; }
        bool HasLogin { get; }
        bool CanLogin { get; }
        bool CanLoginWithWebView { get; }
        string LoginWebViewUrl { get; }
        Dictionary<string, string> Cookie { get; }
        Dictionary<string, string> ImageRequestHeader { get; }
        string Host { get; }
        IApiConfig ApiConfig { get; }
        SearchOptionBase GenerateSearchOptionBase { get; }
        bool WebViewLoginHandler(string url, string cookie);

        Task<(int MaxPage, IEnumerable<IGalleryModel> Gallery)> Gallery(int page = 0,
            SearchOptionBase searchOption = null);

        Task<bool> Login(string userName, string password);

        Task<IGalleryDetailModel> Detail(IGalleryModel model);
    }
}