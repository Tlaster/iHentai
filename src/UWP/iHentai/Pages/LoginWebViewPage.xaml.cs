using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using iHentai.Mvvm;
using iHentai.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginWebViewPage : IMvvmView<LoginWebViewViewModel>
    {
        public LoginWebViewPage()
        {
            this.InitializeComponent();
        }

        public new LoginWebViewViewModel ViewModel
        {
            get => (LoginWebViewViewModel) base.ViewModel;
            set => base.ViewModel = value;
        }

        private async void WebView_OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            var cookit = await sender.InvokeScriptAsync("eval", new[] { "document.cookie" });
            ViewModel.WebViewNavigated(args.Uri.ToString(), cookit);
        }
    }
}
