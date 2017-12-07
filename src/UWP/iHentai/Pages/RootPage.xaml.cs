using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using iHentai.Helpers;
using iHentai.Mvvm;
using iHentai.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Pages
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RootPage
    {
        private UISettings _uiSettings;

        public RootPage()
        {
            InitializeComponent();
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            TitleBar.Height = coreTitleBar.Height;
            Window.Current.SetTitleBar(TitleBar);
            NavigationService.Frame = RootFrame;
            NavigationService.NavigateViewModel<ServiceSelectionViewModel>();
            // Register a handler for when the size of the overlaid caption control changes.
            // For example, when the app moves to a screen with a different DPI.
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
            ThemeHelper.AccentColorUpdated(TitleBar);
            _uiSettings = new UISettings();
            _uiSettings.ColorValuesChanged += RootPage_ColorValuesChanged;
            // Register a handler for when the title bar visibility changes.
            // For example, when the title bar is invoked in full screen mode.
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
            // Get the size of the caption controls area and back button 
            // (returned in logical pixels), and move your content around as necessary.
            LeftPaddingColumn.Width = new GridLength(coreTitleBar.SystemOverlayLeftInset);
            RightPaddingColumn.Width = new GridLength(coreTitleBar.SystemOverlayRightInset);

            // Update title bar control size as needed to account for system size changes.
            TitleBar.Height = coreTitleBar.Height;
        }

        private void CoreTitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
            TitleBar.Visibility = sender.IsVisible ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}