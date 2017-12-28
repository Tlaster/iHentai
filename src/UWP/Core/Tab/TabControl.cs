using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Windows.Devices.Input;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Tab
{
    public sealed class TabControl : Control
    {
        public static readonly DependencyProperty ItemsTemplateProperty = DependencyProperty.Register(
            nameof(ItemsTemplate), typeof(DataTemplate), typeof(TabControl),
            new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            nameof(ItemsSource), typeof(IEnumerable), typeof(TabControl),
            new PropertyMetadata(default(IEnumerable), OnItemsSourceChanged));

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            nameof(SelectedItem), typeof(object), typeof(TabControl),
            new PropertyMetadata(default(object), OnSelectedItemChanged));

        public static readonly DependencyProperty AddCommandProperty = DependencyProperty.Register(
            nameof(AddCommand), typeof(ICommand), typeof(TabControl), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty TabBackgroundProperty = DependencyProperty.Register(
            nameof(TabBackground), typeof(Brush), typeof(TabControl),
            new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            nameof(Header), typeof(object), typeof(TabControl), new PropertyMetadata(default(object)));

        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
            nameof(HeaderTemplate), typeof(DataTemplate), typeof(TabControl),
            new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            nameof(Content), typeof(object), typeof(TabControl), new PropertyMetadata(default(object)));

        private readonly Dictionary<object, object> _items = new Dictionary<object, object>();

        private readonly ConcurrentDictionary<object, RenderTargetBitmap>
            _previews = new ConcurrentDictionary<object, RenderTargetBitmap>();

        private Button _addButton;
        private ListView _tabList;

        public TabControl()
        {
            DefaultStyleKey = typeof(TabControl);
        }

        public Brush TabBackground
        {
            get => (Brush) GetValue(TabBackgroundProperty);
            set => SetValue(TabBackgroundProperty, value);
        }

        public ICommand AddCommand
        {
            get => (ICommand) GetValue(AddCommandProperty);
            set => SetValue(AddCommandProperty, value);
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

        public IEnumerable ItemsSource
        {
            get => (IEnumerable) GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

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

        public DataTemplate ItemsTemplate
        {
            get => (DataTemplate) GetValue(ItemsTemplateProperty);
            set => SetValue(ItemsTemplateProperty, value);
        }

        public Grid TabListRoot { get; private set; }

        public Grid TabListBackground { get; private set; }

        public event EventHandler TabClosed;
        public event EventHandler AddRequest;

        private static void OnSelectedItemChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            (dependencyObject as TabControl)?.OnSelectedItemChanged(e.NewValue, e.OldValue);
        }

        private async void OnSelectedItemChanged(object newValue, object oldValue)
        {
            if (newValue == null || !_items.ContainsKey(newValue)) return;

            if (oldValue != null)
            {
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
            }

            Content = _items[newValue];
        }

        private static void OnItemsSourceChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            (dependencyObject as TabControl)?.ItemsSourceChanged(e.NewValue as IEnumerable, e.OldValue as IEnumerable);
        }

        private void ItemsSourceChanged(IEnumerable newValue, IEnumerable oldValue)
        {
            _items.Clear();
            SelectedItem = null;
            if (oldValue is INotifyCollectionChanged oldCollection)
                oldCollection.CollectionChanged -= OnItemsSourceCollectionChanged;
            if (newValue is INotifyCollectionChanged newCollection)
                newCollection.CollectionChanged += OnItemsSourceCollectionChanged;
            if (newValue != null)
            {
                foreach (var item in newValue) AddContent(item);
                SelectedItem = _items.LastOrDefault().Key;
            }
        }

        private void AddContent(object item)
        {
            var content = ItemsTemplate.LoadContent();
            if (content is FrameworkElement frameworkElement) frameworkElement.DataContext = item;
            _items.Add(item, content);
        }

        private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null && e.NewItems.Count > 0)
                    {
                        foreach (var item in e.NewItems) AddContent(item);
                        SelectedItem = ItemsSource.Cast<object>().LastOrDefault();
                    }

                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null && e.OldItems.Count > 0)
                    {
                        var shouldChangeSelectedItem = false;
                        foreach (var item in e.OldItems)
                        {
                            shouldChangeSelectedItem = SelectedItem == item ||
                                                       SelectedItem == item &&
                                                       ItemsSource.Cast<object>().LastOrDefault() == item;
                            _items.Remove(item);
                        }

                        if (shouldChangeSelectedItem) SelectedItem = ItemsSource.Cast<object>().LastOrDefault();
                    }

                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldItems != null && e.OldItems.Count > 0)
                        foreach (var item in e.OldItems)
                            _items.Remove(item);
                    if (e.NewItems != null && e.NewItems.Count > 0)
                    {
                        foreach (var item in e.NewItems) AddContent(item);
                        SelectedItem = e.NewItems[0];
                    }
                    else
                    {
                        SelectedItem = SelectedItem = ItemsSource.Cast<object>().LastOrDefault();
                    }

                    break;
                case NotifyCollectionChangedAction.Reset:
                    _items.Clear();
                    if (sender is IEnumerable enumerable)
                    {
                        foreach (var item in enumerable) AddContent(item);
                        SelectedItem = SelectedItem = ItemsSource.Cast<object>().LastOrDefault();
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _tabList = GetTemplateChild("TabList") as ListView;
            TabListRoot = GetTemplateChild("TabListRoot") as Grid;
            TabListBackground = GetTemplateChild("TabListBackground") as Grid;
            _tabList.SetBinding(Selector.SelectedItemProperty, new Binding
            {
                Source = this,
                Path = new PropertyPath(nameof(SelectedItem)),
                Mode = BindingMode.TwoWay
            });
            _tabList.SetBinding(ItemsControl.ItemsSourceProperty, new Binding
            {
                Source = this,
                Path = new PropertyPath(nameof(ItemsSource)),
                Mode = BindingMode.OneWay
            });
            _tabList.ContainerContentChanging += TabList_ContainerContentChanging;
            _tabList.ChoosingItemContainer += TabList_ChoosingItemContainer;
            _addButton = GetTemplateChild("AddButton") as Button;
            _addButton.Click += AddButton_Click;
            SelectedItem = _items.LastOrDefault().Key;
            OnSelectedItemChanged(SelectedItem, null);
        }

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
            if (!(ToolTipService.GetToolTip(container) is TabPreview toolTip))
            {
                toolTip = new TabPreview();
                ToolTipService.SetToolTip(container, toolTip);
                ToolTipService.SetPlacement(container, PlacementMode.Bottom);
                ToolTipService.SetPlacementTarget(container, toolTip);
            }

            var prevText = toolTip.FindDescendant<TextBlock>();
            var text = container.FindDescendant<TextBlock>();
            Binding binding;
            if (string.IsNullOrEmpty(TitlePath))
                binding = new Binding
                {
                    Source = item,
                    Mode = BindingMode.OneWay
                };
            else if (TitlePath.StartsWith("self.", StringComparison.CurrentCultureIgnoreCase))
                binding = new Binding
                {
                    Source = _items[item],
                    Path = new PropertyPath(TitlePath.Substring("self.".Length)),
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
            toolTip.DataContext = _items[item];
        }

        private void TabList_ChoosingItemContainer(ListViewBase sender, ChoosingItemContainerEventArgs args)
        {
            var container = args.ItemContainer ?? new ListViewItem();
            container.DataContext = args.Item;
            container.Loaded += ContainerItemLoaded;
            container.PointerReleased += Container_PointerReleased;
            container.PointerEntered += ContainerOnPointerEntered;
            args.ItemContainer = container;
        }

        private void ContainerOnPointerEntered(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            var container = sender as ContentControl;
            var item = container?.Content;
            if (ToolTipService.GetToolTip(container) is TabPreview toolTip && item != null &&
                _items.TryGetValue(item, out var element) && element is UIElement uiElement)
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
            container.Loaded -= ContainerItemLoaded;
            var item = container.Content;
            UpdateContainer(container, item);
            var button = container.FindDescendant<Button>();
            button.Click += ButtonClick;
        }

        private void Container_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var ptr = e.Pointer;
            if (ptr.PointerDeviceType == PointerDeviceType.Mouse)
            {
                var ptrPt = e.GetCurrentPoint(sender as ListViewItem);
                if (ptrPt.Properties.PointerUpdateKind == PointerUpdateKind.MiddleButtonReleased)
                {
                    CloseTab((sender as ListViewItem).Content);
                    (sender as ListViewItem).PointerReleased -= Container_PointerReleased;
                    e.Handled = true;
                }
            }
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            (sender as Button).Click -= ButtonClick;
            var item = (sender as Button).DataContext;
            CloseTab(item);
        }

        private void CloseTab(object item)
        {
            (ItemsSource as IList)?.Remove(item);
            TabClosed?.Invoke(this, EventArgs.Empty);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddRequest?.Invoke(this, EventArgs.Empty);
            if (AddCommand != null && AddCommand.CanExecute(null)) AddCommand.Execute(null);
        }
    }
}