using iHentai.Core.Common.Controls;
using iHentai.Core.ViewModels;
using Xamarin.Forms.Xaml;

namespace iHentai.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WebLoginPage
    {
        public WebLoginPage()
        {
            InitializeComponent();
        }

        private void ExWebView_OnNavigated(object sender, CookieNavigatedEventArgs e)
        {
            (ViewModel as WebLoginViewModel).WebViewNavigated(e.Url, e.Cookies);
        }
    }
}