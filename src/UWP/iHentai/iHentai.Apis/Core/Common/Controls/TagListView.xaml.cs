using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace iHentai.Apis.Core.Common.Controls
{
    public class TagClickEventArgs
    {
        public TagClickEventArgs(object item, object originalSource)
        {
            Item = item;
            OriginalSource = originalSource;
        }

        public object Item { get; }

        public object OriginalSource { get; }
    }

    public sealed partial class TagListView
    {
        public TagListView()
        {
            InitializeComponent();
        }

        public FlyoutBase Flyout { get; set; }

        public event EventHandler<TagClickEventArgs> TagClick;

        private void UIElement_OnRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if (Flyout is MenuFlyout menu && menu.Items != null)
            {
                foreach (var item in menu.Items)
                {
                    item.DataContext = (sender as FrameworkElement)?.DataContext;
                }
            }
            Flyout?.ShowAt(e.OriginalSource as FrameworkElement);
            TagClick?.Invoke(this, new TagClickEventArgs((sender as FrameworkElement)?.DataContext, e.OriginalSource));
        }

        private void UIElement_OnHolding(object sender, HoldingRoutedEventArgs e)
        {
            if (Flyout is MenuFlyout menu && menu.Items != null)
            {
                foreach (var item in menu.Items)
                {
                    item.DataContext = (sender as FrameworkElement)?.DataContext;
                }
            }
            Flyout?.ShowAt(e.OriginalSource as FrameworkElement);
            TagClick?.Invoke(this, new TagClickEventArgs((sender as FrameworkElement)?.DataContext, e.OriginalSource));
        }
    }
}