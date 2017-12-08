using System;
using Windows.UI.Xaml.Controls;

namespace iHentai.Extensions
{
    public static class ScrollViewerExtensions
    {
        public static void ScrollToProportion(this ScrollViewer scrollViewer, double scrollViewerOffsetProportion)
        {
            // Update the Horizontal and Vertical offset
            if (scrollViewer == null)
                return;

            var scrollViewerHorizontalOffset = scrollViewerOffsetProportion * scrollViewer.ScrollableWidth;
            var scrollViewerVerticalOffset = scrollViewerOffsetProportion * scrollViewer.ScrollableHeight;

            scrollViewer.ChangeView(scrollViewerHorizontalOffset, scrollViewerVerticalOffset, null);
        }

        public static double GetScrollViewerOffsetProportion(this ScrollViewer scrollViewer)
        {
            if (scrollViewer == null)
                return 0;

            var horizontalOffsetProportion = scrollViewer.ScrollableWidth == 0
                ? 0
                : scrollViewer.HorizontalOffset / scrollViewer.ScrollableWidth;
            var verticalOffsetProportion = scrollViewer.ScrollableHeight == 0
                ? 0
                : scrollViewer.VerticalOffset / scrollViewer.ScrollableHeight;

            var scrollViewerOffsetProportion = Math.Max(horizontalOffsetProportion, verticalOffsetProportion);
            return scrollViewerOffsetProportion;
        }
    }
}