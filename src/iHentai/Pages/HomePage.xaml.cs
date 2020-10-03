using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using iHentai.Common;
using iHentai.Extensions;
using iHentai.Extensions.Models;
using iHentai.Pages.Script;
using iHentai.Services.Models.Script;
using iHentai.ViewModels.Script;
using NavigationView = Microsoft.UI.Xaml.Controls.NavigationView;
using NavigationViewBackRequestedEventArgs = Microsoft.UI.Xaml.Controls.NavigationViewBackRequestedEventArgs;
using NavigationViewDisplayMode = Microsoft.UI.Xaml.Controls.NavigationViewDisplayMode;
using NavigationViewDisplayModeChangedEventArgs = Microsoft.UI.Xaml.Controls.NavigationViewDisplayModeChangedEventArgs;
using NavigationViewItem = Microsoft.UI.Xaml.Controls.NavigationViewItem;
using NavigationViewItemBase = Microsoft.UI.Xaml.Controls.NavigationViewItemBase;
using NavigationViewPaneClosingEventArgs = Microsoft.UI.Xaml.Controls.NavigationViewPaneClosingEventArgs;
using NavigationViewSelectionChangedEventArgs = Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238


namespace iHentai.Pages
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            var manager = this.Resolve<IExtensionManager?>();
            if (manager != null)
            {
                manager.Extensions.CollectionChanged += (sender, args) =>
                {
                    UpdateMenuItems();
                };
                UpdateMenuItems();
                ExtensionMenuItem.SelectsOnInvoked = manager is INetworkExtensionManager;
            }
            RootNavigationView.SelectedItem = RootNavigationView.MenuItems[0];
            CoreApplication.GetCurrentView().TitleBar.LayoutMetricsChanged += (s, e) => UpdateAppTitle(s);
        }

        void UpdateMenuItems()
        {
            ExtensionMenuItem.MenuItemsSource = this.Resolve<IExtensionManager>().Extensions
                .OrderBy(it => it.Name)
                .Select(it => new NavigationViewItem
                {
                    Tag = it,
                    Content = it.Name
                });
        }
        private void UpdateAppTitle(CoreApplicationViewTitleBar coreTitleBar)
        {
            var currMargin = AppTitleBar.Margin;
            AppTitleBar.Margin = new Thickness(currMargin.Left, currMargin.Top, coreTitleBar.SystemOverlayRightInset,
                currMargin.Bottom);
            RootFrame.Margin = new Thickness(0, AppTitleBar.Height, 0, 0);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Window.Current.SetTitleBar(AppTitleBar);
        }

        private async void NavigationView_SelectionChanged(NavigationView sender,
            NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                RootFrame.Navigate(typeof(SettingsPage));
                RootFrame.BackStack.Clear();
            }
            else if (args.SelectedItem is FrameworkElement element)
            {
                switch (element.Tag)
                {
                    case string value:
                        RootFrame.Navigate(GetType().Assembly.GetType(GetType().Namespace + "." + value));
                        RootFrame.BackStack.Clear();
                        break;
                    case ExtensionManifest value:
                        var api = await this.Resolve<IExtensionManager>().GetApi(value);
                        if (api != null)
                        {
                            if (api.RequireLogin())
                            {
                                RootFrame.Navigate(typeof(ScriptLoginPage), new ScriptLoginViewModel(api));
                            }
                            else
                            {
                                RootFrame.Navigate(typeof(ScriptGalleryPage), new ScriptGalleryViewModel(api));
                            }
                            RootFrame.BackStack.Clear();
                        }
                        break;
                }
            }
        }

        private void RootNavigationView_OnBackRequested(NavigationView sender,
            NavigationViewBackRequestedEventArgs args)
        {
            if (RootFrame.CanGoBack)
            {
                RootFrame.GoBack();
            }
        }

        private void NavigationViewControl_PaneClosing(NavigationView sender, NavigationViewPaneClosingEventArgs args)
        {
        }

        private void NavigationViewControl_PaneOpened(NavigationView sender, object args)
        {
        }

        private void NavigationViewControl_DisplayModeChanged(NavigationView sender,
            NavigationViewDisplayModeChangedEventArgs args)
        {
            var currMargin = AppTitleBar.Margin;
            if (sender.DisplayMode == NavigationViewDisplayMode.Minimal)
            {
                AppTitleBar.Margin = new Thickness(sender.CompactPaneLength * 2, currMargin.Top, currMargin.Right,
                    currMargin.Bottom);
            }
            else
            {
                AppTitleBar.Margin = new Thickness(sender.CompactPaneLength, currMargin.Top, currMargin.Right,
                    currMargin.Bottom);
            }
        }
    }
}