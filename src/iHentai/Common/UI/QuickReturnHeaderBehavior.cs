using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI.Behaviors;
using Microsoft.Toolkit.Uwp.UI.Extensions;

namespace iHentai.Common.UI
{
    internal class QuickReturnHeaderBehavior2 : BehaviorBase<FrameworkElement>
    {
        public static readonly DependencyProperty HeaderElementProperty = DependencyProperty.Register(
            nameof(HeaderElement), typeof(UIElement), typeof(QuickReturnHeaderBehavior2),
            new PropertyMetadata(null, PropertyChangedCallback));

        private ScrollViewer _scrollViewer;
        private double _scrollPosition;

        public UIElement HeaderElement
        {
            get => (UIElement) GetValue(HeaderElementProperty);
            set => SetValue(HeaderElementProperty, value);
        }

        protected override bool Initialize()
        {
            var result = AssignAnimation();
            return result;
        }

        protected override bool Uninitialize()
        {
            RemoveAnimation();
            return true;
        }

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var b = d as QuickReturnHeaderBehavior2;
            b?.AssignAnimation();
        }

        private bool AssignAnimation()
        {
            if (AssociatedObject == null)
            {
                return false;
            }

            if (_scrollViewer == null)
            {
                _scrollViewer = AssociatedObject as ScrollViewer ?? AssociatedObject.FindDescendant<ScrollViewer>();
            }

            if (_scrollViewer == null)
            {
                return false;
            }

            if (!(HeaderElement is FrameworkElement headerElement))
            {
                return false;
            }

            _scrollViewer.ViewChanged -= ScrollViewer_ViewChanged;
            _scrollViewer.ViewChanged += ScrollViewer_ViewChanged;

            headerElement.SizeChanged -= ScrollHeader_SizeChanged;
            headerElement.SizeChanged += ScrollHeader_SizeChanged;

            return true;
        }

        private void RemoveAnimation()
        {
            if (_scrollViewer != null)
            {
                _scrollViewer.ViewChanged -= ScrollViewer_ViewChanged;
            }

            if (HeaderElement is FrameworkElement element)
            {
                element.SizeChanged -= ScrollHeader_SizeChanged;
            }
        }


        private void ScrollHeader_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AssignAnimation();
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var header = (FrameworkElement) HeaderElement;
            if (_scrollPosition < _scrollViewer.VerticalOffset)
            {
                // scrolling down
                header.Visibility = Visibility.Collapsed;
            }
            else if (_scrollPosition > _scrollViewer.VerticalOffset)
            {
                // scrolling up
                header.Visibility = Visibility.Visible;
            }
            _scrollPosition = _scrollViewer.VerticalOffset;
        }
    }
}