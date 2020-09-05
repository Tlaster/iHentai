using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using iHentai.Common;
using iHentai.Common.Helpers;
using Microsoft.Toolkit.Uwp.UI;

namespace iHentai
{
    sealed partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            HentaiApp.Instance.Init();
            ImageCache.Instance.InitializeAsync(httpMessageHandler: HentaiHttpHandler.Instance);
            ProgressImageCache.Instance.InitializeAsync(httpMessageHandler: HentaiHttpHandler.Instance);
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (!(Window.Current.Content is RootView rootView))
            {
                rootView = new RootView();
                Window.Current.Content = rootView;
            }

            if (e.PrelaunchActivated == false)
            {
                Window.Current.Activate();
            }
        }
    }
}