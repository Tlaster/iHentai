using System;
using Windows.UI.Xaml.Controls;
using iHentai.Core.ViewModels;
using iHentai.Mvvm;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Core.Pages
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginWebViewPage : IMvvmView<LoginWebViewViewModel>
    {
        public LoginWebViewPage()
        {
            InitializeComponent();
        }

        public new LoginWebViewViewModel ViewModel
        {
            get => (LoginWebViewViewModel) base.ViewModel;
            set => base.ViewModel = value;
        }

        private async void WebView_OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            var cookit = await sender.InvokeScriptAsync("eval", new[] {"document.cookie"});
            ViewModel.WebViewNavigated(args.Uri.ToString(), cookit);
        }
    }
}