using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace iHentai.Controls.Book
{

    public sealed class BookView : Control
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource", typeof(object), typeof(BookView),
            new PropertyMetadata(default, OnItemsSourceChanged));

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
            "ItemTemplate", typeof(DataTemplate), typeof(BookView),
            new PropertyMetadata(default, OnItemTemplateChanged));

        public static readonly DependencyProperty DataTemplateSelectorProperty = DependencyProperty.Register(
            "DataTemplateSelector", typeof(DataTemplateSelector), typeof(BookView),
            new PropertyMetadata(default, OnDataTemplateSelectorChanged));

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
            "SelectedIndex", typeof(int), typeof(BookView), new PropertyMetadata(default(int)));

        private FlipView _contentFlipView;
        private PageTemplateSelector _selector;

        public BookView()
        {
            DefaultStyleKey = typeof(BookView);
        }

        public int SelectedIndex
        {
            get => (int) GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        public DataTemplateSelector DataTemplateSelector
        {
            get => (DataTemplateSelector) GetValue(DataTemplateSelectorProperty);
            set => SetValue(DataTemplateSelectorProperty, value);
        }

        internal ObservableCollection<List<object>> FlipViewSource { get; } = new ObservableCollection<List<object>>();

        public object ItemsSource
        {
            get => GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate) GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        private static void OnItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as BookView).OnItemTemplateChanged(e.NewValue as DataTemplate);
        }

        private void OnItemTemplateChanged(DataTemplate value)
        {
            if (_selector != null) _selector.Template = value;
        }

        private static void OnDataTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as BookView).OnDataTemplateSelectorChanged(e.NewValue as DataTemplateSelector);
        }

        private void OnDataTemplateSelectorChanged(DataTemplateSelector value)
        {
            if (_selector != null) _selector.Selector = value;
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as BookView).OnItemsSourceChanged(e.NewValue, e.OldValue);
        }

        private void OnItemsSourceChanged(object newValue, object oldValue)
        {
            if (oldValue is INotifyCollectionChanged oldNotifyCollectionChanged)
                oldNotifyCollectionChanged.CollectionChanged -= OnItemsSourceChanged;

            if (newValue is INotifyCollectionChanged newNotifyCollectionChanged)
                newNotifyCollectionChanged.CollectionChanged += OnItemsSourceChanged;

            if (newValue is IEnumerable enumerable)
            {
                FlipViewSource.Clear();
                var resultIndex = -1;
                var index = 0;
                foreach (var item in enumerable)
                {
                    if (index % 2 == 0)
                    {
                        resultIndex++;
                        FlipViewSource.Add(new List<object> {item});
                    }
                    else
                    {
                        FlipViewSource[resultIndex].Add(item);
                    }

                    index++;
                }
            }
        }

        private void OnItemsSourceChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //TODO:
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var rootGrid = GetTemplateChild("RootGrid") as Grid;
            _selector = rootGrid.Resources["PageTemplateSelector"] as PageTemplateSelector;
            _selector.Template = ItemTemplate;
            _contentFlipView = GetTemplateChild("ContentFlipView") as FlipView;
            _contentFlipView.SetBinding(ItemsControl.ItemsSourceProperty, new Binding
            {
                Source = FlipViewSource
            });
        }
    }

    internal class IndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (int)value / 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (int)value * 2;
        }
    }

    internal class PageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Template { get; set; }
        public DataTemplateSelector Selector { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (Selector != null) return Selector.SelectTemplate(item, container);
            return Template;
        }
    }}
