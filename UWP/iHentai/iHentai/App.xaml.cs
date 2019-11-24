using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace iHentai
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (!(Window.Current.Content is RootView))
            {
                Window.Current.Content = new RootView();
            }

            if (!e.PrelaunchActivated)
            {
                Window.Current.Activate();
            }
        }
    }
}
