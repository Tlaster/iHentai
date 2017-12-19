using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Conet.Pages;
using Conet.ViewModels;
using iHentai.Basic.Helpers;
using iHentai.Paging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Pages
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RootPage
    {
        private readonly UISettings _uiSettings;

        public RootPage()
        {
            InitializeComponent();
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            TitleBar.Height = coreTitleBar.Height;
            Window.Current.SetTitleBar(TitleBar);
            RootFrame.NavigateAsync(typeof(ApiSelectionPage), new ApiSelectionViewModel());
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