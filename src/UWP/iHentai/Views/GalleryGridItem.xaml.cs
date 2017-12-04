using System;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using iHentai.Apis.Core.Models.Interfaces;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using WaterFallView;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace iHentai.Views
{
    public class GalleryItemResizer : IItemResizer
    {
        public Size Resize(object item, Size oldSize, Size availableSize)
        {
            if (!(item is IGalleryModel))
                return availableSize;
            var model = (IGalleryModel) item;
            if (model.ThumbHeight < 0d || model.ThumbWidth < 0d)
                return availableSize;
            var size = new Size(availableSize.Width,
                model.ThumbHeight * 1d / (model.ThumbWidth * 1d) * availableSize.Width);
            return size;
        }
    }

    public sealed partial class GalleryGridItem : UserControl
    {
        public GalleryGridItem()
        {
            InitializeComponent();
        }

        public event EventHandler<IGalleryModel> MoreInfoRequest;

        protected override void OnHolding(HoldingRoutedEventArgs e)
        {
            base.OnHolding(e);
            e.Handled = true;
            MoreInfoRequest?.Invoke(this, DataContext as IGalleryModel);
        }

        protected override void OnRightTapped(RightTappedRoutedEventArgs e)
        {
            base.OnRightTapped(e);
            e.Handled = true;
            MoreInfoRequest?.Invoke(this, DataContext as IGalleryModel);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (!(DataContext is IGalleryModel))
                return base.MeasureOverride(availableSize);
            var model = (IGalleryModel) DataContext;
            if (model.ThumbHeight < 0d || model.ThumbWidth < 0d)
                return base.MeasureOverride(availableSize);
            var size = new Size(availableSize.Width,
                model.ThumbHeight * 1d / (model.ThumbWidth * 1d) * availableSize.Width);
            VisualEx.SetCenterPoint(RootGrid, $"{size.Width / 2}, {size.Height / 2}, 0");
            return size;
        }

        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            base.OnPointerExited(e);
            var animation = new OpacityAnimation {To = 0, Duration = TimeSpan.FromMilliseconds(1200)};
            animation.StartAnimation(ShadowPanel);

            var parentAnimation = new ScaleAnimation {To = "1", Duration = TimeSpan.FromMilliseconds(1200)};
            parentAnimation.StartAnimation(RootGrid);
        }

        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
            {
                ShadowPanel.Visibility = Visibility.Visible;
                var animation = new OpacityAnimation {To = 1, Duration = TimeSpan.FromMilliseconds(600)};
                animation.StartAnimation(ShadowPanel);

                var parentAnimation = new ScaleAnimation {To = "1.1", Duration = TimeSpan.FromMilliseconds(600)};
                parentAnimation.StartAnimation(RootGrid);
            }
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            base.OnPointerPressed(e);
            var parentAnimation = new ScaleAnimation {Duration = TimeSpan.FromMilliseconds(600), To = "1"};
            parentAnimation.StartAnimation(RootGrid);
        }

        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            base.OnPointerReleased(e);
            var parentAnimation = new ScaleAnimation {Duration = TimeSpan.FromMilliseconds(1200), To = "1.1"};
            parentAnimation.StartAnimation(RootGrid);
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            MoreInfoRequest?.Invoke(this, DataContext as IGalleryModel);
        }
    }
}