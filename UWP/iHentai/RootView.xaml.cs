using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.System;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using iHentai.Activities;
using iHentai.Common;
using iHentai.Common.Tab;
using Microsoft.Toolkit.Helpers;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace iHentai
{
    public sealed partial class RootView
    {
        private Grid _titleBarGrid;
        TabManager TabManager { get; } = new TabManager();
        public RootView()
        {
            this.InitializeComponent();
            CoreApplication.GetCurrentView().TitleBar.Also(it =>
            {
                it.ExtendViewIntoTitleBar = true;
                it.LayoutMetricsChanged += OnCoreTitleBarOnLayoutMetricsChanged;
            });
            ApplicationView.GetForCurrentView().TitleBar.Also(it =>
            {
                it.ButtonBackgroundColor = Colors.Transparent;
                it.ButtonInactiveBackgroundColor = Colors.Transparent;
            });
            Singleton<BroadcastCenter>.Instance.Subscribe("tab_toggle_visible", (o, o1) => { ToggleTabBar(); });
            TabManager.TabItems.CollectionChanged += TabItemsOnCollectionChanged;
        }

        private void TabItemsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (TabManager.Count > 1)
            {
                if (!RootTabView.IsVisible())
                {
                    ToggleTabBar();
                }
            }
            else
            {
                if (RootTabView.IsVisible())
                {
                    ToggleTabBar();
                }
            }
        }

        private void ToggleTabBar()
        {
            RootTabView.Visibility = RootTabView.IsVisible() ? Visibility.Collapsed : Visibility.Visible;
            SecondaryTitleBar.Visibility = RootTabView.IsVisible() ? Visibility.Collapsed : Visibility.Visible;
            Window.Current.SetTitleBar(RootTabView.IsVisible() ? _titleBarGrid : SecondaryTitleBar);
        }

        private void OnCoreTitleBarOnLayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            if (FlowDirection == FlowDirection.LeftToRight)
            {
                ShellTitlebarEndInset.MinWidth = sender.SystemOverlayRightInset;
                ShellTitlebarInset.MinWidth = sender.SystemOverlayLeftInset;
            }
            else
            {
                ShellTitlebarEndInset.MinWidth = sender.SystemOverlayLeftInset;
                ShellTitlebarInset.MinWidth = sender.SystemOverlayRightInset;
            }

            ShellTitlebarEndInset.Height = ShellTitlebarInset.Height = SecondaryTitleBar.Height = sender.Height;
        }

        private void RootTabView_AddTabButtonClick(TabView sender, object args)
        {
            AddTab();
        }

        private void AddTab()
        {
            TabManager.AddDefault();
            if (TabManager.Count > 0)
            {
                RootTabView.SelectedIndex = TabManager.Count - 1;
            }
        }

        private void RootTabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            if (args.Item is ITabItem item)
            {
                TabManager.Remove(item);
            }
        }

        private void NavigateToNumberedTabKeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            args.Handled = true;
            var index = args.KeyboardAccelerator.Key switch
            {
                VirtualKey.Number1 => 0,
                VirtualKey.Number2 => 1,
                VirtualKey.Number3 => 2,
                VirtualKey.Number4 => 3,
                VirtualKey.Number5 => 4,
                VirtualKey.Number6 => 5,
                VirtualKey.Number7 => 6,
                VirtualKey.Number8 => 7,
                VirtualKey.Number9 => 8,
                _ => 0,
            };
            if (index > TabManager.Count - 1)
            {
                return;
            }

            RootTabView.SelectedIndex = index;
        }

        private void NewTabKeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            args.Handled = true;
            AddTab();
        }

        private void CloseSelectedTabKeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            args.Handled = true;
            if (RootTabView.SelectedItem is ITabItem item)
            {
                TabManager.Remove(item);
            }
        }

        private void RootTabView_Loaded(object sender, RoutedEventArgs e)
        {
            var tabContainerGrid = RootTabView.FindDescendantByName("TabContainerGrid") as Grid;
            if (_titleBarGrid == null)
            {
                _titleBarGrid = new Grid {Background = new SolidColorBrush(Colors.Transparent)};
                SetColumnSpan(_titleBarGrid, 4);
                tabContainerGrid.Children.Insert(0, _titleBarGrid);
            }
            ToggleTabBar();
        }

    }
}
