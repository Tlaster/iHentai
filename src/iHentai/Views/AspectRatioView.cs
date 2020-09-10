using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace iHentai.Views
{
    class AspectRatioView : Panel
    {
        public static readonly DependencyProperty HeightRequestProperty = DependencyProperty.Register(
            nameof(HeightRequest), typeof(int), typeof(AspectRatioView), new PropertyMetadata(default(int)));

        public static readonly DependencyProperty WidthRequestProperty = DependencyProperty.Register(
            nameof(WidthRequest), typeof(int), typeof(AspectRatioView), new PropertyMetadata(default(int)));

        public int WidthRequest
        {
            get => (int)GetValue(WidthRequestProperty);
            set => SetValue(WidthRequestProperty, value);
        }

        public int HeightRequest
        {
            get => (int)GetValue(HeightRequestProperty);
            set => SetValue(HeightRequestProperty, value);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var availableWidth = finalSize.Width;
            if (WidthRequest == 0 || HeightRequest == 0)
            {
                foreach (var item in Children)
                {
                    item.Arrange(new Rect(0, 0, finalSize.Width, height: finalSize.Height));
                }

                return Children.Max(it => it.DesiredSize);
            }

            var requestHeight = Convert.ToDouble(HeightRequest) / Convert.ToDouble(WidthRequest) *
                                Convert.ToDouble(availableWidth);
            requestHeight = Math.Min(requestHeight, finalSize.Height);
            availableWidth = Convert.ToDouble(WidthRequest) / Convert.ToDouble(HeightRequest) *
                              Convert.ToDouble(requestHeight);
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
                foreach (var item in Children)
                {
                    item.Measure(availableSize);
                }

                return Children.Max(it => it.DesiredSize);
            }

            var requestHeight = Convert.ToDouble(HeightRequest) / Convert.ToDouble(WidthRequest) *
                                Convert.ToDouble(availableWidth);
            requestHeight = Math.Min(requestHeight, availableSize.Height);
            availableWidth = Convert.ToDouble(WidthRequest) / Convert.ToDouble(HeightRequest) *
                              Convert.ToDouble(requestHeight);
            var size = new Size(availableWidth, requestHeight);
            foreach (var item in Children)
            {
                item.Measure(size);
            }

            return size;
        }
    }
}
