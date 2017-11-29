using System;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using iHentai.Apis.Core.Models.Interfaces;
using iHentai.Apis.NHentai.Models;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Extensions;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace iHentai.Views
{
    public sealed partial class GalleryGridItem : UserControl
    {
        public GalleryGridItem()
        {
            this.InitializeComponent();
        }
        
        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                var panel = this.ShadowPanel;
                if (panel != null)
                {
                    panel.Visibility = Visibility.Visible;
                    var animation = new OpacityAnimation { To = 1, Duration = TimeSpan.FromMilliseconds(600) };
                    animation.StartAnimation(panel);

                    var parentAnimation = new ScaleAnimation { To = "1.1", Duration = TimeSpan.FromMilliseconds(600) };
                    parentAnimation.StartAnimation(panel.Parent as UIElement);
                }
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (!(DataContext is IGalleryModel))
            {
                return base.MeasureOverride(availableSize);
            }
            var model = (IGalleryModel) DataContext;
            var size = new Size(availableSize.Width,
                model.ThumbHeight * 1d / (model.ThumbWidth * 1d) * availableSize.Width);
            VisualEx.SetCenterPoint(RootGrid, $"{size.Width / 2}, {size.Height / 2}, 0");
            return size;
        }

        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            base.OnPointerExited(e);
            var panel = this.ShadowPanel;
            if (panel != null)
            {
                var animation = new OpacityAnimation { To = 0, Duration = TimeSpan.FromMilliseconds(1200) };
                animation.StartAnimation(panel);

                var parentAnimation = new ScaleAnimation { To = "1", Duration = TimeSpan.FromMilliseconds(1200) };
                parentAnimation.StartAnimation(panel.Parent as UIElement);
            }
        }
    }
}
