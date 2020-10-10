using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;
using iHentai.Common;
using iHentai.Common.Helpers;
using iHentai.Data;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Toolkit.Uwp.UI;

namespace iHentai
{
    sealed partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            AppCenter.Start("8ab592bb-9953-4ecc-a72a-7cf799f92903",
                typeof(Analytics), typeof(Crashes));
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

        protected override void OnFileActivated(FileActivatedEventArgs args)
        {
            if (!(Window.Current.Content is RootView rootView))
            {
                rootView = new RootView();
                Window.Current.Content = rootView;
            }

            if (args.Files.FirstOrDefault() is StorageFile file)
            {
                rootView.ReadFile(file);
            }

            if (args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                Window.Current.Activate();
            }
        }
    }
}