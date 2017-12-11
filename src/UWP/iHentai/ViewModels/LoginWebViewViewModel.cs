﻿using iHentai.Apis.Core;
using iHentai.Mvvm;

namespace iHentai.ViewModels
{
    public class LoginWebViewViewModel : ViewModel<bool>
    {
        public LoginWebViewViewModel(IHentaiApis api)
        {
            Api = api;
        }

        public IHentaiApis Api { get; }

        public void WebViewNavigated(string url, string cookie)
        {
            if (Api.WebViewLoginHandler(url, cookie))
                Close(true);
        }
    }
}