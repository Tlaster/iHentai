using System.Diagnostics;
using Foundation;
using iHentai.Core.Common.Controls;
using iHentai.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExWebView), typeof(ExWebViewRenderer))]

namespace iHentai.iOS.Renderers
{
    public class ExWebViewRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
                Delegate = new WebViewDelegate(Element as ExWebView);
            if (e.OldElement != null)
                Delegate = null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                Delegate = null;
            base.Dispose(disposing);
        }
    }

    internal class WebViewDelegate : UIWebViewDelegate
    {
        private readonly ExWebView _exWebView;

        public WebViewDelegate(ExWebView exWebView)
        {
            _exWebView = exWebView;
        }

        public override bool ShouldStartLoad(UIWebView webView, NSUrlRequest request,
            UIWebViewNavigationType navigationType)
        {
            webView.UserInteractionEnabled = false;
            _exWebView.OnNavigating(new CookieNavigationEventArgs
            {
                Url = request.Url.AbsoluteString
            });
            return true;
        }

        public override void LoadStarted(UIWebView webView)
        {
            // TODO: Add code here to possible redirect
        }

        public override void LoadFailed(UIWebView webView, NSError error)
        {
            // TODO: Display Error Here
            Debug.WriteLine("ERROR: {0}", error.ToString());
        }

        public override void LoadingFinished(UIWebView webView)
        {
            _exWebView.OnNavigated(new CookieNavigatedEventArgs
            {
                Cookies = webView.EvaluateJavascript("document.cookie"),
                Url = webView.Request.Url.AbsoluteString
            });
        }
    }
}