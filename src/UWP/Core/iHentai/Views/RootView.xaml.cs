using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using iHentai.Basic.Extensions;
using iHentai.Paging;
using iHentai.Services;
using Microsoft.Toolkit.Uwp.UI.Animations;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace iHentai.Views
{
    public sealed partial class RootView
    {
        private readonly SplashScreen _splash;
        private Rect _splashImageRect;

        public RootView(SplashScreen splashScreen, Type defaultNavItem)
        {
            InitializeComponent();
            DefaultNavItem = defaultNavItem;
            _splash = splashScreen;
            if (_splash != null)
            {
                _splashImageRect = _splash.ImageLocation;
                _splash.Dismissed += SplashScreenOnDismissed;
                PositionImage();
            }
            else
            {
                ExtendedSplash.Visibility = Visibility.Collapsed;
            }
            RootFrame.Navigated += RootFrameOnNavigated;
            //ExtendAcrylicIntoTitleBar();
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
        }

        private void RootFrameOnNavigated(object sender, HentaiNavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = RootFrame.CanGoBack
                ? AppViewBackButtonVisibility.Visible
                : AppViewBackButtonVisibility.Collapsed;
        }

        public Type DefaultNavItem { get; }

        private void ExtendAcrylicIntoTitleBar()
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonForegroundColor = Colors.Gray;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (RootFrame.CanGoBack)
            {
                RootFrame.GoBackAsync();
                e.Handled = true;
            }
        }

        private async void SplashScreenOnDismissed(SplashScreen sender, object args)
        {
            _splash.Dismissed -= SplashScreenOnDismissed;
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                DismissExtendedSplash);
        }

        private async void DismissExtendedSplash()
        {
            await Task.Delay(1000);
            RootFrame
                .Scale(1.1f, 1.1f, (float) (RootFrame.ActualWidth / 2f), (float) (RootFrame.ActualHeight / 2f), 0D)
                .StartAsync()
                .FireAndForget();
            await ExtendedSplashImage.Scale(0.8f, 0.8f, (float) (ExtendedSplashImage.ActualWidth / 2f),
                (float) (ExtendedSplashImage.ActualHeight / 2f), 250d).StartAsync();
            ExtendedSplashImage
                .Scale(10f, 10f, (float) (ExtendedSplashImage.ActualWidth / 2f),
                    (float) (ExtendedSplashImage.ActualHeight / 2f), 500d, 0d, EasingType.Back)
                .StartAsync()
                .FireAndForget();
            RootFrame
                .Scale(1f, 1f, (float) (RootFrame.ActualWidth / 2f), (float) (RootFrame.ActualHeight / 2f), delay: 125d,
                    easingType: EasingType.Quintic)
                .StartAsync()
                .FireAndForget();
            await ExtendedSplash
                .Fade(delay: 125d)
                .StartAsync();
            ExtendedSplash.Visibility = Visibility.Collapsed;

            await WhatsNewDisplayService.ShowIfAppropriateAsync();
            await FirstRunDisplayService.ShowIfAppropriateAsync();
        }

        private void PositionImage()
        {
            ExtendedSplashImage.SetValue(Canvas.LeftProperty, _splashImageRect.Left);
            ExtendedSplashImage.SetValue(Canvas.TopProperty, _splashImageRect.Top);
            //if (StaticResource.IsPhone)
            //{
            //    extendedSplashImage.Height = splashImageRect.Height / ScaleFactor;
            //    extendedSplashImage.Width = splashImageRect.Width / ScaleFactor;
            //}
            //else
            //{
            ExtendedSplashImage.Height = _splashImageRect.Height;
            ExtendedSplashImage.Width = _splashImageRect.Width;
            //}
        }
    }
}