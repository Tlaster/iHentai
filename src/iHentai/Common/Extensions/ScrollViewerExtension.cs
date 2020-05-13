using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using iHentai.Common.Collection;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Microsoft.UI.Xaml.Controls;

namespace iHentai.Common.Extensions
{
    class ScrollViewerExtension
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.RegisterAttached(
            "ItemsSource", typeof(object), typeof(ScrollViewer), new PropertyMetadata(default(object)));

        public static object GetItemsSource(DependencyObject element)
        {
            return element.GetValue(ItemsSourceProperty);
        }

        public static void SetItemsSource(DependencyObject element, object value)
        {
            if (!(element is ScrollViewer scrollViewer))
            {
                return;
            }

            if (element.GetValue(ItemsSourceProperty) is ISupportIncrementalLoading oldLoading)
            {
                scrollViewer.ViewChanged -= ScrollViewerOnViewChanged;
            }

            element.SetValue(ItemsSourceProperty, value);
            
            if (value is ISupportIncrementalLoading loading)
            {
                if (scrollViewer.GetContentControl() is FrameworkElement childElement)
                {
                    childElement.SizeChanged += (sender, args) => TryLoadIfNotFill(scrollViewer);
                }
                //scrollViewer.RegisterPropertyChangedCallback(ScrollViewer.ScrollableHeightProperty,
                //    OnScrollableHeightChanged);
                //scrollViewer.SizeChanged += ScrollViewerOnSizeChanged;
                scrollViewer.ViewChanged += ScrollViewerOnViewChanged;
                Task.Delay(100).ContinueWith(it => DispatcherHelper.ExecuteOnUIThreadAsync(() => TryLoadIfNotFill(scrollViewer)));
            }

        }


        private static async void TryLoadIfNotFill(ScrollViewer scrollViewer)
        {
            if (GetIsLoading(scrollViewer))
            {
                return;
            }

            if (scrollViewer.ScrollableHeight > scrollViewer.ActualHeight)
            {
                return;
            }

            if (!(GetItemsSource(scrollViewer) is ISupportIncrementalLoading loading) || !loading.HasMoreItems)
            {
                return;
            }

            SetIsLoading(scrollViewer, true);
            await loading.LoadMoreItemsAsync(20);
            SetIsLoading(scrollViewer, false);
        }

        private static async void ScrollViewerOnViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (!(sender is ScrollViewer dependencyObject))
            {
                return;
            }
            if (e.IsIntermediate && !GetIsLoading(dependencyObject))
            {
                var scroller = (ScrollViewer) sender;
                var distanceToEnd = scroller.ExtentHeight - (scroller.VerticalOffset + scroller.ViewportHeight);
                // trigger if within 2 viewports of the end
                if (distanceToEnd <= 2.0 * scroller.ViewportHeight
                    && dependencyObject.GetValue(ItemsSourceProperty) is ISupportIncrementalLoading loading && loading.HasMoreItems)
                {
                    SetIsLoading(dependencyObject, true);
                    await loading.LoadMoreItemsAsync(20);
                    SetIsLoading(dependencyObject, false);
                }
            }
        }

        private static readonly DependencyProperty IsLoadingProperty = DependencyProperty.RegisterAttached(
            "IsLoading", typeof(bool), typeof(ScrollViewerExtension), new PropertyMetadata(default(bool)));

        private static void SetIsLoading(DependencyObject element, bool value)
        {
            element.SetValue(IsLoadingProperty, value);
        }

        private static bool GetIsLoading(DependencyObject element)
        {
            return (bool) element.GetValue(IsLoadingProperty);
        }
    }
}
