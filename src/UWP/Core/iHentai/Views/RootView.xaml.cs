using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using iHentai.Basic.Helpers;
using iHentai.Paging;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace iHentai.Views
{
    public sealed partial class RootView
    {
        private readonly UISettings _uiSettings;
        public RootView()
        {
            this.InitializeComponent();
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            TitleBar.Height = coreTitleBar.Height;
            Window.Current.SetTitleBar(TitleBar);
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
            ThemeHelper.AccentColorUpdated(TitleBar);
            _uiSettings = new UISettings();
            _uiSettings.ColorValuesChanged += RootPage_ColorValuesChanged;
            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;
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
