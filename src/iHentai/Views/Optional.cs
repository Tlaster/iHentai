using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace iHentai.Views
{
    internal class Optional : Grid
    {
        public static readonly DependencyProperty WhenProperty = DependencyProperty.Register(
            "When", typeof(bool), typeof(Optional), new PropertyMetadata(default(bool), OnPropertyChangedCallback));

        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register(
            "ContentTemplate", typeof(DataTemplate), typeof(Optional),
            new PropertyMetadata(default(DataTemplate), OnPropertyChangedCallback));

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content", typeof(object), typeof(Optional), new PropertyMetadata(default, OnPropertyChangedCallback));

        private UIElement _element;

        public bool When
        {
            get => (bool) GetValue(WhenProperty);
            set => SetValue(WhenProperty, value);
        }

        public DataTemplate ContentTemplate
        {
            get => (DataTemplate) GetValue(ContentTemplateProperty);
            set => SetValue(ContentTemplateProperty, value);
        }

        public object Content
        {
            get => (object) GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        private static void OnPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Optional view)
            {
                if (e.Property == WhenProperty)
                {
                    view.OnWhenChanged((bool) e.NewValue);
                }

                if (e.Property == ContentProperty)
                {
                    view.OnContentChanged(e.NewValue);
                }
            }
        }

        private void OnContentChanged(object newValue)
        {
            if (_element != null && _element is FrameworkElement element && element.DataContext != newValue)
            {
                element.DataContext = newValue;
            }
        }

        private void OnWhenChanged(bool value)
        {
            if (value)
            {
                if (ContentTemplate != null)
                {
                    _element = ContentTemplate.GetElement(new ElementFactoryGetArgs
                    {
                        Data = Content,
                        Parent = this
                    });
                    if (_element is FrameworkElement element && element.DataContext != Content)
                    {
                        element.DataContext = Content;
                    }

                    Children.Add(_element);
                }
            }
            else
            {
                if (ContentTemplate != null && _element != null)
                {
                    ContentTemplate.RecycleElement(new ElementFactoryRecycleArgs
                    {
                        Element = _element,
                        Parent = this
                    });
                    _element = null;
                }

                Children.Clear();
            }
        }
    }
}