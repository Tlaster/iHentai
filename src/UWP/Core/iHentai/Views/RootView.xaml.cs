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
using iHentai.Basic;
using iHentai.Basic.Extensions;
using iHentai.Basic.Helpers;
using iHentai.Mvvm;
using iHentai.Services;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Tab;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace iHentai.Views
{
    public class WindowGenerator : IWindowGenerator
    {
        public Type DefaultNavItem { get; set; }

        public UIElement GetNewWindowElement()
        {
            return new RootView(null, DefaultNavItem);
        }
    }

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
            ExtendAcrylicIntoTitleBar();
            Loaded += RootView_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            Singleton<MessagingCenter>.Instance.Subscribe<NewTabArgs>(this, NewTabArgs.NewTab, args =>
            {
                var content = new MvvmFrame();
                tab.AddContent(content);
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
                content.Navigate(args.ViewModel, args.Params);
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
            });
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
            if (tab.Content is MvvmFrame frame && frame.CanGoBack)
            {
                frame.GoBackAsync();
                e.Handled = true;
            }
        }

        private void RootView_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= RootView_Loaded;
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            tab.TabListRoot.Height = coreTitleBar.Height;
            Window.Current.SetTitleBar(tab.TabListBackground);
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;
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
            tab
                .Scale(1.1f, 1.1f, (float) (tab.ActualWidth / 2f), (float) (tab.ActualHeight / 2f), 0D)
                .StartAsync()
                .FireAndForget();
            await ExtendedSplashImage.Scale(0.8f, 0.8f, (float) (ExtendedSplashImage.ActualWidth / 2f),
                (float) (ExtendedSplashImage.ActualHeight / 2f), 250d).StartAsync();
            ExtendedSplashImage
                .Scale(10f, 10f, (float) (ExtendedSplashImage.ActualWidth / 2f),
                    (float) (ExtendedSplashImage.ActualHeight / 2f), 500d, 0d, EasingType.Back)
                .StartAsync()
                .FireAndForget();
            tab
                .Scale(1f, 1f, (float) (tab.ActualWidth / 2f), (float) (tab.ActualHeight / 2f), delay: 125d,
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

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            UpdateTitleBarLayout(sender);
        }

        private void UpdateTitleBarLayout(CoreApplicationViewTitleBar coreTitleBar)
        {
            tab.TabHeaderPadding = new Thickness(coreTitleBar.SystemOverlayLeftInset, 0,
                coreTitleBar.SystemOverlayRightInset, 0);
            tab.TabListRoot.Height = coreTitleBar.Height;
        }

        private void CoreTitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
            tab.TabListRoot.Visibility = sender.IsVisible ? Visibility.Visible : Visibility.Collapsed;
            tab.TabListBackground.Visibility = sender.IsVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Tab_OnTabClosed(object sender, TabCloseEventArgs e)
        {
            if (e.TabCount == 0) Application.Current.Exit();
        }

        private async void MenuFlyoutItem_OnClick(object sender, RoutedEventArgs e)
        {
            var newViewId = 0;
            await CoreApplication.CreateNewView().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var content = new RootView(null, DefaultNavItem);
                Window.Current.Content = content;
                Window.Current.Activate();
                newViewId = ApplicationView.GetForCurrentView().Id;
            });
            var viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
        }
    }
}