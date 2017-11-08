using System;
using System.Collections.Generic;
using System.Net;
using Xamarin.Forms;

namespace iHentai.Core.Common.Controls
{
    public class CookieNavigationEventArgs
        : EventArgs
    {
        public string Url { get; set; }
    }

    public class CookieNavigatedEventArgs
        : CookieNavigationEventArgs
    {
        public string Cookies { get; set; }
        public WebNavigationEvent NavigationMode { get; set; }
    }


    public class ExWebView : WebView
    {
        public string Cookies { get; protected set; }

        public new event EventHandler<CookieNavigatedEventArgs> Navigated;
        public new event EventHandler<CookieNavigationEventArgs> Navigating;

        public virtual void OnNavigated(CookieNavigatedEventArgs args)
        {
            Cookies = args.Cookies;
            Navigated?.Invoke(this, args);
        }

        public virtual void OnNavigating(CookieNavigationEventArgs args)
        {
            Navigating?.Invoke(this, args);
        }
    }
}