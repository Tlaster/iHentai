using iHentai.Apis.Core;
using iHentai.Mvvm;

namespace iHentai.Core.ViewModels
{
    public class LoginWebViewViewModel : ViewModel<bool>
    {
        public LoginWebViewViewModel(ILoginApi api)
        {
            Api = api;
        }

        public ILoginApi Api { get; }

        public void WebViewNavigated(string url, string cookie)
        {
            if (Api.WebViewLoginHandler(url, cookie))
                Close(true);
        }
    }
}