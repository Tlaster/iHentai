using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using iHentai.Common;
using iHentai.Common.Tab;
using Microsoft.Toolkit.Helpers;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Microsoft.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace iHentai
{
    public sealed partial class RootView : INotifyPropertyChanged
    {
        private bool _isUpdatingTab;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                if (!_isUpdatingTab)
                {
                    _selectedTabIndex = value;
                }
            }
        }

        private Grid _titleBarGrid;
        private int _selectedTabIndex;
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
            SystemNavigationManager.GetForCurrentView().Also(it =>
            {
                it.AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
                it.BackRequested += OnBackRequested;
            });
            Singleton<BroadcastCenter>.Instance.Subscribe("tab_toggle_visible", (o, o1) => { ToggleTabBar(); });
            TabManager.TabItems.CollectionChanged += TabItemsOnCollectionChanged;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            var handled = false;
            var container = ContentPivot.ContainerFromIndex(ContentPivot.SelectedIndex);
            if (container is PivotItem pivotItem && pivotItem.ContentTemplateRoot is IHistoricalTabItem item)
            {
                handled = true;
                item.GoBack();
            }

            if (!handled)
            {
                handled = TabManager.Count > 1;
            }

            e.Handled = handled;
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
                SecondaryTitleBarEndInset.MinWidth = sender.SystemOverlayRightInset;
                SecondaryTitleBarInset.MinWidth = sender.SystemOverlayLeftInset;
            }
            else
            {
                ShellTitlebarEndInset.MinWidth = sender.SystemOverlayLeftInset;
                ShellTitlebarInset.MinWidth = sender.SystemOverlayRightInset;
                SecondaryTitleBarEndInset.MinWidth = sender.SystemOverlayLeftInset;
                SecondaryTitleBarInset.MinWidth = sender.SystemOverlayRightInset;
            }

            ShellTitlebarEndInset.Height = ShellTitlebarInset.Height = SecondaryTitleBar.Height = sender.Height;
        }

        private void RootTabView_AddTabButtonClick(TabView sender, object args)
        {
            AddTab();
        }
        
        private void RootTabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            var index = sender.TabItems.IndexOf(args.Item);
            RemoveTab(index);
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

            SelectedTabIndex = index;
        }

        private void NewTabKeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            args.Handled = true;
            AddTab();
        }

        private void CloseSelectedTabKeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            args.Handled = true;
            RemoveTab(SelectedTabIndex);
        }

        private void RootTabView_Loaded(object sender, RoutedEventArgs e)
        {
            RootTabView.Loaded -= RootTabView_Loaded;
            var tabContainerGrid = RootTabView.FindDescendantByName("TabContainerGrid") as Grid;
            if (_titleBarGrid == null)
            {
                _titleBarGrid = new Grid {Background = new SolidColorBrush(Colors.Transparent)};
                SetColumnSpan(_titleBarGrid, 4);
                tabContainerGrid.Children.Insert(0, _titleBarGrid);
                ToggleTabBar();
                RootTabView.FindDescendantByName("AddButton").Margin = new Thickness();
            }
        }

        private void ContentPivot_Loaded(object sender, RoutedEventArgs e)
        {
            ContentPivot.FindDescendantByName("HeaderClipper").Visibility = Visibility.Collapsed;
            ContentPivot.FindDescendantByName("LeftHeaderPresenter").Visibility = Visibility.Collapsed;
            ContentPivot.FindDescendantByName("PreviousButton").Visibility = Visibility.Collapsed;
            ContentPivot.FindDescendantByName("NextButton").Visibility = Visibility.Collapsed;
            ContentPivot.FindDescendantByName("RightHeaderPresenter").Visibility = Visibility.Collapsed;
        }

        private void SwitchTabKeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            SwitchTab();
        }

        private void AddTab()
        {
            _isUpdatingTab = true;
            TabManager.AddDefault();
            _isUpdatingTab = false;
            if (TabManager.Count > 0)
            {
                SelectedTabIndex = TabManager.Count - 1;
            }
        }

        private void RemoveTab(int index)
        {
            if (TabManager.Count > 1)
            {
                if (SelectedTabIndex == index)
                {
                    SelectedTabIndex = Math.Min(SelectedTabIndex + 1, TabManager.Count - 1);
                }
                _isUpdatingTab = true;
                TabManager.Remove(index);
                _isUpdatingTab = false;
            }
        }

        private void SwitchTab()
        {
            if (TabManager.Count < 2)
            {
                return;
            }

            var nextIndex = SelectedTabIndex + 1;
            if (nextIndex > TabManager.Count - 1)
            {
                nextIndex = 0;
            }

            SelectedTabIndex = nextIndex;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
