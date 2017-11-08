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
            if (e.OldElement != null)
            {
                Control.NavigationCompleted -= ControlOnNavigationCompleted;
                Control.NavigationStarting -= ControlOnNavigationStarting;
            }
            if (e.NewElement != null)
            {
                Control.NavigationCompleted += ControlOnNavigationCompleted;
                Control.NavigationStarting += ControlOnNavigationStarting;
            }
        }

        private async void ControlOnNavigationCompleted(Windows.UI.Xaml.Controls.WebView sender,
            WebViewNavigationCompletedEventArgs args)
        {
            (Element as ExWebView).OnNavigated(new CookieNavigatedEventArgs
            {
                Cookies = sender.InvokeScript("eval", new[] {"document.cookie"}),
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
        }
    }
}