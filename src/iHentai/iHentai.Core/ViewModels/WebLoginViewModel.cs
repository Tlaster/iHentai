using iHentai.Core.Views;
using iHentai.Mvvm;
using iHentai.Services.Core;

namespace iHentai.Core.ViewModels
{
    [Page(typeof(WebLoginPage))]
    public class WebLoginViewModel : ViewModel<bool>
    {
        public WebLoginViewModel(IHentaiApis apis)
        {
            Apis = apis;
        }

        public IHentaiApis Apis { get; }

        public void WebViewNavigated(string url, string cookie)
        {
            if (Apis.WebViewLoginHandler(url, cookie))
                Close(true);
        }
    }
}