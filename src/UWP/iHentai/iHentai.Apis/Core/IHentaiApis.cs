using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using iHentai.Apis.Core.Models.Interfaces;
using iHentai.Services;

namespace iHentai.Apis.Core
{
    public interface ILoginApi
    {
        bool FouceLogin { get; }
        bool HasLogin { get; }
        bool CanLoginWithWebView { get; }
        string LoginWebViewUrl { get; }
        bool WebViewLoginHandler(string url, string cookie);
        Task<bool> WebViewLoginFollowup(CancellationToken cancellationToken = default);
        Task<bool> Login(string userName, string password, CancellationToken cancellationToken = default);
    }

    //public interface ICookieApi
    //{
    //    string Host { get; }
    //    Dictionary<string, string> Cookie { get; }
    //    Dictionary<string, string> RequestHeader { get; }
    //}

    public interface IConfigApi
    {
        IApiConfig ApiConfig { get; }
    }

    public interface IHentaiApi : IApi
    {
        string Host { get; }
        SearchOptionBase SearchOptionGenerator { get; }

        Task<(int MaxPage, IEnumerable<IGalleryModel> Gallery)> Gallery(int page = 0,
            SearchOptionBase searchOption = null, CancellationToken cancellationToken = default);

        Task<IGalleryDetailModel> Detail(IGalleryModel model, CancellationToken cancellationToken = default);
    }

    public interface IWebApi
    {
        string GetWebLink(IGalleryModel model);
    }
}