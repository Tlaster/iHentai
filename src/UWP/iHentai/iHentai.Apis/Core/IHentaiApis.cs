using System.Collections.Generic;
using System.Threading.Tasks;
using iHentai.Apis.Core.Models.Interfaces;

namespace iHentai.Apis.Core
{
    public interface ILoginApi
    {
        bool FouceLogin { get; }
        bool HasLogin { get; }
        bool CanLoginWithWebView { get; }
        string LoginWebViewUrl { get; }
        bool WebViewLoginHandler(string url, string cookie);
        Task<bool> WebViewLoginFollowup();
        Task<bool> Login(string userName, string password);
    }

    public interface ICookieApi
    {
        Dictionary<string, string> Cookie { get; }
        Dictionary<string, string> RequestHeader { get; }
    }

    public interface IConfigApi
    {
        IApiConfig ApiConfig { get; }
    }

    public interface IHentaiApi
    {
        string Host { get; }
        SearchOptionBase SearchOptionGenerator { get; }
        Task<(int MaxPage, IEnumerable<IGalleryModel> Gallery)> Gallery(int page = 0,
            SearchOptionBase searchOption = null);
        Task<IGalleryDetailModel> Detail(IGalleryModel model);
    }

    public interface IWebApi
    {
        string GetWebLink(IGalleryModel model);
    }
}