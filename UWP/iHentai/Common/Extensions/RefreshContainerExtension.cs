using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using iHentai.Common.Collection;

namespace iHentai.Common.Extensions
{
    class RefreshContainerExtension
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.RegisterAttached(
            "ItemsSource", typeof(object), typeof(RefreshContainerExtension), new PropertyMetadata(default(object)));

        public static void SetItemsSource(DependencyObject element, object value)
        {
            if (!(element is RefreshContainer refreshContainer))
            {
                return;
            }

            if (GetItemsSource(element) is ISupportRefresh)
            {
                refreshContainer.RefreshRequested -= RefreshContainerOnRefreshRequested;
            }
            element.SetValue(ItemsSourceProperty, value);
            if (value is ISupportRefresh source)
            {
                refreshContainer.RefreshRequested += RefreshContainerOnRefreshRequested;
                source.Refresh();
            }
        }

        private static async void RefreshContainerOnRefreshRequested(RefreshContainer sender, RefreshRequestedEventArgs args)
        {
            if (GetItemsSource(sender) is ISupportRefresh source)
            {
                using var def = args.GetDeferral();
                await source.Refresh();
                def.Complete();
            }
        }

        public static object GetItemsSource(DependencyObject element)
        {
            return (object) element.GetValue(ItemsSourceProperty);
        }
    }
}
