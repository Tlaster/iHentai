using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace iHentai.Controls
{
    class AspectRatioView : Panel
    {
        public static readonly DependencyProperty HeightRequestProperty = DependencyProperty.Register(
            nameof(HeightRequest), typeof(int), typeof(AspectRatioView), new PropertyMetadata(default(int)));

        public static readonly DependencyProperty WidthRequestProperty = DependencyProperty.Register(
            nameof(WidthRequest), typeof(int), typeof(AspectRatioView), new PropertyMetadata(default(int)));

        public int WidthRequest
        {
            get => (int) GetValue(WidthRequestProperty);
            set => SetValue(WidthRequestProperty, value);
        }

        public int HeightRequest
        {
            get => (int) GetValue(HeightRequestProperty);
            set => SetValue(HeightRequestProperty, value);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var availableWidth = finalSize.Width;
            if (WidthRequest == 0 || HeightRequest == 0)
            {
                return new Size(0, 0);
            }

            var requestHeight = Convert.ToDouble(HeightRequest) / Convert.ToDouble(WidthRequest) *
                                Convert.ToDouble(availableWidth);
            var size = new Size(availableWidth, requestHeight);
            var rect = new Rect(0, 0, size.Width, size.Height);
            foreach (var item in Children)
            {
                item.Arrange(rect);
            }

            return size;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var availableWidth = availableSize.Width;
            if (WidthRequest == 0 || HeightRequest == 0)
            {
                return new Size(0, 0);
            }

            var requestHeight = Convert.ToDouble(HeightRequest) / Convert.ToDouble(WidthRequest) *
                                Convert.ToDouble(availableWidth);
            var size = new Size(availableWidth, requestHeight);
            foreach (var item in Children)
            {
                item.Measure(size);
            }

            return size;
        }
    }
}
