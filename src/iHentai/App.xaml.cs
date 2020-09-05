using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using iHentai.Common;
using iHentai.Common.Helpers;
using Microsoft.Toolkit.Uwp.UI;

namespace iHentai
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
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
