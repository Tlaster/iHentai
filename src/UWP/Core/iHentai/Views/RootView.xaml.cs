using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using iHentai.Basic.Extensions;
using iHentai.Database;
using iHentai.Mvvm;
using iHentai.Services;
using iHentai.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Toolkit.Uwp.UI.Animations;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace iHentai.Views
{
    public sealed partial class RootView
    {
        private readonly Type _defaultNavItem;
        private readonly SplashScreen _splash;
        private Rect _splashImageRect;

        public RootView(SplashScreen splashScreen, Type defaultNavItem)
        {
            InitializeComponent();
            _defaultNavItem = defaultNavItem;
            _splash = splashScreen;
            if (_splash != null)
            {
                _splashImageRect = _splash.ImageLocation;
                _splash.Dismissed += SplashScreenOnDismissed;
                PositionImage();
            }

            Loaded += RootView_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
        }

        public RootViewModel ViewModel { get; private set; }

        private void OnBackRequested(object sender, BackRequestedEventArgs backRequestedEventArgs)
        {
            if (tab.Content is MvvmFrame frame && frame.CanGoBack) frame.GoBackAsync();
        }

        private void RootView_Loaded(object sender, RoutedEventArgs e)
        {
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
            using (var context = new ApplicationDbContext())
            {
                await context.Database.MigrateAsync();
            }

            ViewModel = new RootViewModel(_defaultNavItem);
            DataContext = ViewModel;

            await Task.Delay(3000);
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
            tab.TabListRoot.Padding = new Thickness(coreTitleBar.SystemOverlayLeftInset, 0,
                coreTitleBar.SystemOverlayRightInset, 0);
            tab.TabListRoot.Height = coreTitleBar.Height;
        }

        private void CoreTitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
            tab.TabListRoot.Visibility = sender.IsVisible ? Visibility.Visible : Visibility.Collapsed;
            tab.TabListBackground.Visibility = sender.IsVisible ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}