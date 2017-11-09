using System;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using iHentai.Core.Common.Controls;
using iHentai.UWP.Renderers;
using Xamarin.Forms.Platform.UWP;
using WebView = Xamarin.Forms.WebView;

[assembly: ExportRenderer(typeof(ExWebView), typeof(ExWebViewRenderer))]

namespace iHentai.UWP.Renderers
{
    public class ExWebViewRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                Control.NavigationCompleted += ControlOnNavigationCompleted;
                Control.NavigationStarting += ControlOnNavigationStarting;
            }
        }

        protected override void Dispose(bool disposing)
        {
            Debug.WriteLine("webview disposing");
            if (disposing && Control != null)
            {
                Control.NavigationCompleted -= ControlOnNavigationCompleted;
                Control.NavigationStarting -= ControlOnNavigationStarting;
                Control.Stop();
            }
            base.Dispose(disposing);
        }

        private async void ControlOnNavigationCompleted(Windows.UI.Xaml.Controls.WebView sender,
            WebViewNavigationCompletedEventArgs args)
        {
            var res = await sender.InvokeScriptAsync("eval", new[] {"document.cookie"});
            Debug.WriteLine("webview ControlOnNavigationCompleted");
            (Element as ExWebView).OnNavigated(new CookieNavigatedEventArgs
            {
                Cookies = res,
                Url = args.Uri.ToString()
            });
        }

        private void ControlOnNavigationStarting(Windows.UI.Xaml.Controls.WebView sender,
            WebViewNavigationStartingEventArgs args)
        {
            (Element as ExWebView).OnNavigating(new CookieNavigatedEventArgs
            {
                Url = args.Uri.ToString()
            });
            Debug.WriteLine("webview ControlOnNavigationStarting");
        }
    }
}