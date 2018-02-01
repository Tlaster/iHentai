using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Devices.Input;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using ListViewBase = Windows.UI.Xaml.Controls.ListViewBase;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Tab
{
    //internal sealed class TabModel
    //{
    //    public TabModel(/*object dataContext,*/ object view)
    //    {
    //        //DataContext = dataContext;
    //        View = view;
    //    }

    //    //public object DataContext { get; }
    //    public object View { get; }
    //}

    public interface IWindowGenerator
    {
        UIElement GetNewWindowElement();
    }

    public sealed class TabControl : Control
    {
        public static readonly DependencyProperty ItemsTemplateProperty = DependencyProperty.Register(
            nameof(ItemsTemplate), typeof(DataTemplate), typeof(TabControl),
            new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            nameof(SelectedItem), typeof(object), typeof(TabControl),
            new PropertyMetadata(default, OnSelectedItemChanged));

        public static readonly DependencyProperty TabBackgroundProperty = DependencyProperty.Register(
            nameof(TabBackground), typeof(Brush), typeof(TabControl),
            new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            nameof(Header), typeof(object), typeof(TabControl), new PropertyMetadata(default));

        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
            nameof(HeaderTemplate), typeof(DataTemplate), typeof(TabControl),
            new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            nameof(Content), typeof(object), typeof(TabControl), new PropertyMetadata(default));

        public static readonly DependencyProperty TabHeaderProperty = DependencyProperty.Register(
            nameof(TabHeader), typeof(object), typeof(TabControl), new PropertyMetadata(default));

        public static readonly DependencyProperty TabFooterProperty = DependencyProperty.Register(
            nameof(TabFooter), typeof(object), typeof(TabControl), new PropertyMetadata(default));

        public static readonly DependencyProperty DefaultContentParamProperty = DependencyProperty.Register(
            nameof(DefaultContentParam), typeof(object), typeof(TabControl),
            new PropertyMetadata(default, OnDefaultContentParamChanged));

        public static readonly DependencyProperty TabHeaderPaddingProperty = DependencyProperty.Register(
            nameof(TabHeaderPadding), typeof(Thickness), typeof(TabControl),
            new PropertyMetadata(default(Thickness), OnTabHeaderPaddingChanged));

        private readonly ConcurrentDictionary<object, RenderTargetBitmap>
            _previews = new ConcurrentDictionary<object, RenderTargetBitmap>();

        private ContentPresenter _tabFooter;

        //private Button _addButton;
        private ContentPresenter _tabHeader;
        private ListView _tabList;

        public TabControl()
        {
            DefaultStyleKey = typeof(TabControl);
            SizeChanged += OnSizeChanged;
        }

        public Thickness TabHeaderPadding
        {
            get => (Thickness) GetValue(TabHeaderPaddingProperty);
            set => SetValue(TabHeaderPaddingProperty, value);
        }

        public IWindowGenerator WindowGenerator { get; set; }

        public object DefaultContentParam
        {
            get => GetValue(DefaultContentParamProperty);
            set => SetValue(DefaultContentParamProperty, value);
        }

        public ObservableCollection<DependencyObject> ItemsSource { get; } =
            new ObservableCollection<DependencyObject>();

        public Brush TabBackground
        {
            get => (Brush) GetValue(TabBackgroundProperty);
            set => SetValue(TabBackgroundProperty, value);
        }

        public DataTemplate HeaderTemplate
        {
            get => (DataTemplate) GetValue(HeaderTemplateProperty);
            set => SetValue(HeaderTemplateProperty, value);
        }

        public object Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public string TitlePath { get; set; }

        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public object TabHeader
        {
            get => GetValue(TabHeaderProperty);
            set => SetValue(TabHeaderProperty, value);
        }

        public DataTemplate ItemsTemplate
        {
            get => (DataTemplate) GetValue(ItemsTemplateProperty);
            set => SetValue(ItemsTemplateProperty, value);
        }

        public object TabFooter
        {
            get => GetValue(TabFooterProperty);
            set => SetValue(TabFooterProperty, value);
        }

        public Grid TabListRoot { get; private set; }

        public Grid TabListBackground { get; private set; }

        public ContentPresenter TabContentPresenter { get; set; }

        private static void OnTabHeaderPaddingChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            (dependencyObject as TabControl).OnTabHeaderPaddingChanged((Thickness) e.NewValue);
        }

        private void OnTabHeaderPaddingChanged(Thickness newValue)
        {
            UpdateTabListSize();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_tabList == null || _tabHeader == null || _tabFooter == null) return;

            UpdateTabListSize();
        }

        private void UpdateTabListSize()
        {
            _tabList.MaxWidth = ActualWidth - TabHeaderPadding.Left - TabHeaderPadding.Right - _tabHeader.ActualWidth -
                                _tabFooter.ActualWidth;
        }

        private static void OnDefaultContentParamChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            (dependencyObject as TabControl)?.OnDefaultContentParamChanged(e.NewValue);
        }

        private void OnDefaultContentParamChanged(object newValue)
        {
            if (!ItemsSource.Any()) Add();
        }

        public event EventHandler<TabCloseEventArgs> TabClosed;

        private static void OnSelectedItemChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            (dependencyObject as TabControl)?.OnSelectedItemChanged(e.NewValue, e.OldValue);
        }

        private async void OnSelectedItemChanged(object newValue, object oldValue)
        {
            if (newValue == null || ItemsSource.All(item => item != newValue)) return;
            if (oldValue != null)
                if (_previews.TryGetValue(oldValue, out var value))
                {
                    await value.RenderAsync(Content as UIElement);
                }
                else
                {
                    var bitmap = new RenderTargetBitmap();
                    await bitmap.RenderAsync(Content as UIElement);
                    _previews.TryAdd(oldValue, bitmap);
                }

#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
            Dispatcher.RunAsync(CoreDispatcherPriority.Low, () => _tabList.ScrollIntoView(newValue));
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
            Content = ItemsSource.FirstOrDefault(item => item == newValue);
        }

        public void Add()
        {
            var content = ItemsTemplate.LoadContent();
            if (content is FrameworkElement frameworkElement) frameworkElement.DataContext = DefaultContentParam;
            ItemsSource.Add(content);
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => SelectedItem = ItemsSource.LastOrDefault());
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
        }

        public void AddContent(DependencyObject content)
        {
            ItemsSource.Add(content);
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => SelectedItem = ItemsSource.LastOrDefault());
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
        }

        //        private void Add(TabModel data)
        //        {
        //            ItemsSource.Add(data);
        //#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
        //            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => SelectedItem = ItemsSource.LastOrDefault());
        //#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
        //        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _tabList = GetTemplateChild("TabList") as ListView;
            TabListRoot = GetTemplateChild("TabListRoot") as Grid;
            TabListBackground = GetTemplateChild("TabListBackground") as Grid;
            TabContentPresenter = GetTemplateChild("ContentPresenter") as ContentPresenter;
            _tabHeader = GetTemplateChild("TabHeaderContentPresenter") as ContentPresenter;
            _tabFooter = GetTemplateChild("TabFooterContentPresenter") as ContentPresenter;
            //_addButton = GetTemplateChild("AddButton") as Button;
            //_addButton.Click += AddButtonOnClick;
            _tabList.SetBinding(Selector.SelectedItemProperty, new Binding
            {
                Source = this,
                Path = new PropertyPath(nameof(SelectedItem)),
                Mode = BindingMode.TwoWay
            });
            //_tabList.SetBinding(ItemsControl.ItemsSourceProperty, new Binding
            //{
            //    Source = this,
            //    Path = new PropertyPath(nameof(ItemsSource)),
            //    Mode = BindingMode.OneWay
            //});
            _tabList.ItemsSource = ItemsSource;
            _tabList.ContainerContentChanging += TabList_ContainerContentChanging;
            _tabList.ChoosingItemContainer += TabList_ChoosingItemContainer;
            //_tabList.DragItemsCompleted += TabListOnDragItemsCompleted;
            UpdateTabListSize();
        }

        //private async void TabListOnDragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs e)
        //{
        //    return;
        //    //var res = e.Items.ToList();
        //    ////if (ItemsSource.Count == 1 && !CoreApplication.GetCurrentView().IsMain)
        //    ////{
        //    ////    await ApplicationViewSwitcher.SwitchAsync(((App)Application.Current).Views[0].ViewId, this.ViewId, ApplicationViewSwitchingOptions.ConsolidateViews);
        //    ////}
        //    //if (e.DropResult == DataPackageOperation.None && WindowGenerator != null)
        //    //{
        //    //    var newViewId = 0;
        //    //    await CoreApplication.CreateNewView().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
        //    //    {
        //    //        var content = WindowGenerator.GetNewWindowElement();
        //    //        var tab = content.FindDescendant<TabControl>();
        //    //        Window.Current.Content = content;
        //    //        Window.Current.Activate();
        //    //        newViewId = ApplicationView.GetForCurrentView().Id;
        //    //        //if (tab != null)
        //    //        //{
        //    //        //    tab.ItemsSource.Clear();
        //    //        //    tab.Add(res.FirstOrDefault() as TabModel);
        //    //        //}
        //    //    });
        //    //    var viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
        //    //    if (viewShown) CloseTab(res.FirstOrDefault());
        //    //}
        //}

        //private void AddButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        //{
        //    Add();
        //}

        private void TabList_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (args.InRecycleQueue) return;
            var container = args.ItemContainer as ContentControl;
            var item = container.Content;
            UpdateContainer(container, item);
        }

        private void UpdateContainer(ContentControl container, object item)
        {
            if (item == null)
                return;
            if (!((ToolTipService.GetToolTip(container.FindDescendant<Grid>()) as ToolTip)?.Content is TabPreview
                toolTip)) return;
            var prevText = toolTip.FindDescendant<TextBlock>();
            var text = container.FindDescendant<TextBlock>();
            Binding binding;
            if (string.IsNullOrEmpty(TitlePath))
                binding = new Binding
                {
                    Source = item,
                    Mode = BindingMode.OneWay
                };
            else
                binding = new Binding
                {
                    Source = item,
                    Path = new PropertyPath(TitlePath),
                    Mode = BindingMode.OneWay
                };
            text?.SetBinding(TextBlock.TextProperty, binding);
            prevText.SetBinding(TextBlock.TextProperty, binding);
            toolTip.DataContext = item;
        }

        private void TabList_ChoosingItemContainer(ListViewBase sender, ChoosingItemContainerEventArgs args)
        {
            if (args.ItemContainer == null)
            {
                var container = new ListViewItem();
                container.Loaded += ContainerItemLoaded;
                container.PointerReleased += Container_PointerReleased;
                container.PointerEntered += ContainerOnPointerEntered;
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
                container.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () => container.FindDescendant<Button>().Click += OnClick);
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
                args.ItemContainer = container;
            }

            args.ItemContainer.DataContext = DataContext = args.Item;
        }

        private void OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            CloseTab((sender as Button).FindAscendant<ListViewItem>());
        }

        private void ContainerOnPointerEntered(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            var container = sender as ContentControl;
            var item = container?.Content;
            if (!((ToolTipService.GetToolTip(container.FindDescendant<Grid>()) as ToolTip)
                    ?.Content is TabPreview toolTip) || item == null ||
                !(ItemsSource.FirstOrDefault(x => x == item) is UIElement uiElement)) return;
            if (_previews.TryGetValue(item, out var preview))
            {
                if (SelectedItem != item)
                {
                    toolTip.FindDescendant<Image>().Source = preview;
                }
                else
                {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    preview.RenderAsync(uiElement);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    toolTip.FindDescendant<Image>().Source = preview;
                }
            }
            else
            {
                var bitmap = new RenderTargetBitmap();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                bitmap.RenderAsync(uiElement);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                _previews.TryAdd(item, bitmap);
                toolTip.FindDescendant<Image>().Source = bitmap;
            }
        }

        private void ContainerItemLoaded(object sender, RoutedEventArgs e)
        {
            var container = sender as ContentControl;
            var item = container.Content;
            UpdateContainer(container, item);
        }

        private void Container_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var ptr = e.Pointer;
            if (ptr.PointerDeviceType == PointerDeviceType.Mouse)
            {
                var ptrPt = e.GetCurrentPoint(sender as ListViewItem);
                if (ptrPt.Properties.PointerUpdateKind == PointerUpdateKind.MiddleButtonReleased)
                {
                    CloseTab(sender as ListViewItem);
                    e.Handled = true;
                }
            }
        }

        //private void ButtonClick(object sender, RoutedEventArgs e)
        //{
        //    (sender as Button).Click -= ButtonClick;
        //    var item = (sender as Button).DataContext;
        //    CloseTab(item);
        //}

        private void CloseTab(ListViewItem container)
        {
            container.Loaded -= ContainerItemLoaded;
            container.PointerReleased -= Container_PointerReleased;
            container.PointerEntered -= ContainerOnPointerEntered;

            var item = container.Content;

            var index = (ItemsSource as IList).IndexOf(item);
            if (SelectedItem == item)
            {
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () => SelectedItem = ItemsSource.ElementAtOrDefault(index) ?? ItemsSource.LastOrDefault());
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
            }

            (ItemsSource as IList)?.Remove(item);
            _previews.TryRemove(item, out var _);
            TabClosed?.Invoke(this, new TabCloseEventArgs(ItemsSource.Count));
        }
    }

    public class TabCloseEventArgs : EventArgs
    {
        public TabCloseEventArgs(int tabCount)
        {
            TabCount = tabCount;
        }

        public int TabCount { get; }
    }
}