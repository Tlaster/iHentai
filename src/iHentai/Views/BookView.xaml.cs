using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace iHentai.Views
{
    public sealed partial class BookView : UserControl
    {
        public BookView()
        {
            this.InitializeComponent();
            var leftSelector = Resources["LeftPageTemplateSelector"] as PageTemplateSelector;
            leftSelector.Template = LeftTemplate;
            var rightSelector = Resources["RightPageTemplateSelector"] as PageTemplateSelector;
            rightSelector.Template = RightTemplate;
        }

        public static readonly DependencyProperty CoverFirstProperty = DependencyProperty.Register(
            "CoverFirst", typeof(bool), typeof(BookView), new PropertyMetadata(default(bool)));

        public bool CoverFirst
        {
            get { return (bool) GetValue(CoverFirstProperty); }
            set { SetValue(CoverFirstProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource", typeof(object), typeof(BookView),
            new PropertyMetadata(default, OnItemsSourceChanged));

        public static readonly DependencyProperty LeftTemplateProperty = DependencyProperty.Register(
            "LeftTemplate", typeof(DataTemplate), typeof(BookView), new PropertyMetadata(default(DataTemplate), PropertyChangedCallback));
        
        public static readonly DependencyProperty RightTemplateProperty = DependencyProperty.Register(
            "RightTemplate", typeof(DataTemplate), typeof(BookView), new PropertyMetadata(default(DataTemplate), PropertyChangedCallback));

        public static readonly DependencyProperty DataTemplateSelectorProperty = DependencyProperty.Register(
            "DataTemplateSelector", typeof(DataTemplateSelector), typeof(BookView),
            new PropertyMetadata(default, OnDataTemplateSelectorChanged));

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
            "SelectedIndex", typeof(int), typeof(BookView), new PropertyMetadata(default));

        public DataTemplate LeftTemplate
        {
            get => (DataTemplate) GetValue(LeftTemplateProperty);
            set => SetValue(LeftTemplateProperty, value);
        }

        public DataTemplate RightTemplate
        {
            get => (DataTemplate) GetValue(RightTemplateProperty);
            set => SetValue(RightTemplateProperty, value);
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

        internal ObservableCollection<BookViewItem> FlipViewSource { get; } = new ObservableCollection<BookViewItem>();

        public object ItemsSource
        {
            get => GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
        
        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BookView view)
            {
                if (e.Property == LeftTemplateProperty)
                {
                    var selector = view.Resources["LeftPageTemplateSelector"] as PageTemplateSelector;
                    selector.Template = e.NewValue as DataTemplate;
                }

                if (e.Property == RightTemplateProperty)
                {
                    var selector = view.Resources["RightPageTemplateSelector"] as PageTemplateSelector;
                    selector.Template = e.NewValue as DataTemplate;
                }
            }
        }

        private static void OnDataTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as BookView).OnDataTemplateSelectorChanged(e.NewValue as DataTemplateSelector);
        }

        private void OnDataTemplateSelectorChanged(DataTemplateSelector value)
        {
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as BookView).OnItemsSourceChanged(e.NewValue, e.OldValue);
        }

        private void OnItemsSourceChanged(object newValue, object oldValue)
        {
            if (oldValue is INotifyCollectionChanged oldNotifyCollectionChanged)
            {
                oldNotifyCollectionChanged.CollectionChanged -= OnItemsSourceChanged;
            }

            if (newValue is INotifyCollectionChanged newNotifyCollectionChanged)
            {
                newNotifyCollectionChanged.CollectionChanged += OnItemsSourceChanged;
            }

            if (newValue is IEnumerable enumerable)
            {
                FlipViewSource.Clear();
                var resultIndex = -1;
                var index = 0;
                if (CoverFirst)
                {
                    index++;
                    resultIndex++;
                    FlipViewSource.Add(new BookViewItem { Left = null });
                }
                foreach (var item in enumerable)
                {
                    if (index % 2 == 0)
                    {
                        resultIndex++;
                        FlipViewSource.Add(new BookViewItem { Left = item });
                    }
                    else
                    {
                        FlipViewSource[resultIndex].Right = item;
                    }

                    index++;
                }
            }
        }

        private void OnItemsSourceChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //TODO:
        }
        
    }
    class IndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (int) value / 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (int) value * 2;
        }
    }

    class PageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Template { get; set; }
        public DataTemplateSelector Selector { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (Selector != null)
            {
                return Selector.SelectTemplate(item, container);
            }

            if (item != null)
            {
                return Template;
            }

            return base.SelectTemplateCore(item, container);
        }
    }
    class BookViewItem : INotifyPropertyChanged
    {
        public object Left { get; set; }
        public object Right { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
