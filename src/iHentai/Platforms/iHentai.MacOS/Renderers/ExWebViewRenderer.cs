using System;
using System.Diagnostics;
using System.Net;
using Foundation;
using iHentai.Core.Common.Controls;
using iHentai.MacOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

[assembly: ExportRenderer(typeof(ExWebView), typeof(ExWebViewRenderer))]

namespace iHentai.MacOS.Renderers
{
    public class ExWebViewRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);
        }
    }

}