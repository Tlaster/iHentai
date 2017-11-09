using Android.Graphics;
using Android.Webkit;
using iHentai.Core.Common.Controls;
using iHentai.Droid.Renderers;
using Java.Lang;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using WebView = Xamarin.Forms.WebView;

[assembly: ExportRenderer(typeof(ExWebView), typeof(ExWebViewRenderer))]

namespace iHentai.Droid.Renderers
{
    public class ExWebViewRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                Control.Settings.JavaScriptEnabled = true;
                Control.SetWebViewClient(new ExWebViewClient(Element as ExWebView));
            }
            if (e.OldElement != null)
            {
                Control.SetWebViewClient(null);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Control?.SetWebViewClient(null);
            }
            base.Dispose(disposing);
        }
    }

    internal class ExWebViewClient : WebViewClient
    {
        private readonly ExWebView _exWebView;

        internal ExWebViewClient(ExWebView exWebView)
        {
            _exWebView = exWebView;
        }

        public override void OnPageStarted(Android.Webkit.WebView view, string url, Bitmap favicon)
        {
            base.OnPageStarted(view, url, favicon);

            _exWebView.OnNavigating(new CookieNavigationEventArgs
            {
                Url = url
            });
        }

        public override void OnPageFinished(Android.Webkit.WebView view, string url)
        {
            view.EvaluateJavascript("document.cookie", new Callback
            {
                ReceiveValue = value =>
                {
                    _exWebView.OnNavigated(new CookieNavigatedEventArgs
                    {
                        Cookies = value.ToString(),
                        Url = url
                    });
                }
            });
        }
    }

    internal class Callback : Object, IValueCallback
    {
        public System.Action<Object> ReceiveValue { get; set; }

        public void OnReceiveValue(Object value)
        {
            ReceiveValue?.Invoke(value);
        }
    }
}