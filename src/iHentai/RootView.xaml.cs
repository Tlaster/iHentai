using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using iHentai.Common;
using iHentai.Common.Helpers;
using iHentai.Pages;
using iHentai.ViewModels.Archive;
using iHentai.Views;
using Microsoft.Toolkit.Uwp.UI;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace iHentai
{
    public sealed partial class RootView
    {
        public RootView()
        {
            InitializeComponent();
            Init();
            RequestedTheme = SettingsManager.Instance.Theme;
            SettingsManager.Instance.ThemeChanged += InstanceOnThemeChanged;
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            ApplicationView.GetForCurrentView().TitleBar.Also(it =>
            {
                it.ButtonBackgroundColor = Colors.Transparent;
                it.ButtonInactiveBackgroundColor = Colors.Transparent;
            });
            UpdateTitlebarButtonColor();
            ActualThemeChanged += OnActualThemeChanged;
            RootFrame.SourcePageType = typeof(HomePage);
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
        }

        private async Task Init()
        {
            HentaiApp.Instance.Init();
            await ImageCache.Instance.InitializeAsync(httpMessageHandler: HentaiHttpHandler.Instance);
            await ProgressImageCache.Instance.InitializeAsync(httpMessageHandler: HentaiHttpHandler.Instance);
            await ImageEx2.WriteableImageCache.Instance.InitializeAsync(httpMessageHandler: HentaiHttpHandler.Instance);
        }

        internal Frame ContentFrame => RootFrame;

        private void InstanceOnThemeChanged(object sender, ElementTheme e)
        {
            RequestedTheme = e;
        }

        private void OnActualThemeChanged(FrameworkElement sender, object args)
        {
            UpdateTitlebarButtonColor();
        }

        private void UpdateTitlebarButtonColor()
        {
            switch (ActualTheme)
            {
                case ElementTheme.Default:
                case ElementTheme.Light:
                    ApplicationView.GetForCurrentView().TitleBar
                        .Also(it => { it.ButtonForegroundColor = Colors.Black; });
                    break;
                case ElementTheme.Dark:
                    ApplicationView.GetForCurrentView().TitleBar.Also(it =>
                    {
                        it.ButtonForegroundColor = Colors.DarkGray;
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (RootFrame.CanGoBack)
            {
                e.Handled = true;
                RootFrame.GoBack();
            }
        }

        private void RootFrame_OnNavigated(object sender, NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = RootFrame.CanGoBack
                ? AppViewBackButtonVisibility.Visible
                : AppViewBackButtonVisibility.Collapsed;
        }

        public void ReadFile(StorageFile file)
        {
            ContentFrame.Navigate(typeof(ReadingPage), new ArchiveReadingViewModel(file));
        }
    }
}