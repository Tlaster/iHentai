using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using iHentai.Basic.Extensions;
using iHentai.Basic.Helpers;
using iHentai.Paging;
using Microsoft.Toolkit.Uwp.UI.Animations;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace iHentai.Views
{
    public sealed partial class RootView
    {
        private readonly UISettings _uiSettings;
        private readonly SplashScreen _splash;
        private Rect _splashImageRect;

        public RootView(SplashScreen splashScreen)
        {
            this.InitializeComponent();
            _splash = splashScreen;
            if (_splash != null)
            {
                _splashImageRect = _splash.ImageLocation;
                _splash.Dismissed += SplashScreenOnDismissed;
                PositionImage();
            }
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            TitleBar.Height = coreTitleBar.Height;
            Window.Current.SetTitleBar(TitleBar);
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
            ThemeHelper.AccentColorUpdated(TitleBar);
            _uiSettings = new UISettings();
            _uiSettings.ColorValuesChanged += RootPage_ColorValuesChanged;
            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;
        }

        private async void SplashScreenOnDismissed(SplashScreen sender, object args)
        {
            _splash.Dismissed -= SplashScreenOnDismissed;

            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, DismissExtendedSplash);
        }

        private async void DismissExtendedSplash()
        {
            await Task.Delay(3000);
            RootFrame
                .Scale(1.25f, 1.25f, (float) (RootFrame.ActualWidth / 2f), (float) (RootFrame.ActualHeight / 2f), 0D)
                .StartAsync()
                .FireAndForget();
            await ExtendedSplashImage.Scale(0.8f, 0.8f, (float) (ExtendedSplashImage.ActualWidth / 2f),
                (float) (ExtendedSplashImage.ActualHeight / 2f), 250d).StartAsync();
            ExtendedSplashImage
                .Scale(10f, 10f, (float) (ExtendedSplashImage.ActualWidth / 2f), (float) (ExtendedSplashImage.ActualHeight / 2f), 1000d, 0d, EasingType.Back)
                .StartAsync()
                .FireAndForget();
            RootFrame
                .Scale(1f, 1f, (float)(RootFrame.ActualWidth / 2f), (float)(RootFrame.ActualHeight / 2f), delay: 250d, easingType: EasingType.Quintic)
                .StartAsync()
                .FireAndForget();
            await ExtendedSplash
                .Fade(delay: 250d)
                .StartAsync();
            ExtendedSplash.Visibility = Visibility.Collapsed;
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

        private async void RootPage_ColorValuesChanged(UISettings sender, object args)
        {
            await Dispatcher.TryRunAsync(CoreDispatcherPriority.Low, () => ThemeHelper.AccentColorUpdated(TitleBar));
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            UpdateTitleBarLayout(sender);
        }

        private void UpdateTitleBarLayout(CoreApplicationViewTitleBar coreTitleBar)
        {
            LeftPaddingColumn.Width = new GridLength(coreTitleBar.SystemOverlayLeftInset);
            RightPaddingColumn.Width = new GridLength(coreTitleBar.SystemOverlayRightInset);
            TitleBar.Height = coreTitleBar.Height;
        }

        private void CoreTitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
            TitleBar.Visibility = sender.IsVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void RootFrame_OnNavigated(object sender, HentaiNavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = RootFrame.CanGoBack
                ? AppViewBackButtonVisibility.Visible
                : AppViewBackButtonVisibility.Collapsed;
        }
    }
}
