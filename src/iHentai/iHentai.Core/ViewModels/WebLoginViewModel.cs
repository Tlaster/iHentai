using System;
using System.Collections.Generic;
using System.Text;
using iHentai.Core.Views;
using iHentai.Mvvm;
using iHentai.Services.Core;

namespace iHentai.Core.ViewModels
{
    [Page(typeof(WebLoginPage))]
    public class WebLoginViewModel : ViewModel<bool>
    {
        public IHentaiApis Apis { get; }
        public WebLoginViewModel(IHentaiApis apis)
        {
            Apis = apis;
        }

        public void WebViewNavigated(string url, string cookie)
        {
            if (Apis.WebViewLoginHandler(url, cookie))
            {
                this.Close(true);
            }
        }
    }
}
