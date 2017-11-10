using System;
using Xamarin.Forms;

namespace iHentai.Core.Common.Controls
{
    public class ScrollChangedEventArgs : EventArgs
    {
        public double VerticalOffset { get; set; }
        public double HorizontalOffset { get; set; }
        public double VerticalChanged { get; set; }
        public double HorizontalChanged { get; set; }
    }

    public class ExListView : ListView
    {
        private double _prevHorizontalOffset;
        private double _prevVerticalOffset;
        public event EventHandler LoadMoreRequest;
        public event EventHandler<ScrollChangedEventArgs> ScrollChanged;

        public void RequestLoadMore()
        {
            LoadMoreRequest?.Invoke(this, EventArgs.Empty);
        }

        public void InvokeScrollChanged(double verticalOffset, double horizontalOffset)
        {
            ScrollChanged?.Invoke(this, new ScrollChangedEventArgs
            {
                VerticalOffset = verticalOffset,
                HorizontalOffset = horizontalOffset,
                VerticalChanged = _prevVerticalOffset - verticalOffset,
                HorizontalChanged = _prevHorizontalOffset - horizontalOffset
            });
            _prevHorizontalOffset = horizontalOffset;
            _prevVerticalOffset = verticalOffset;
//            Debug.WriteLine($"verticalOffset : {verticalOffset}");
//            Debug.WriteLine($"horizontalOffset : {horizontalOffset}");
        }
    }
}