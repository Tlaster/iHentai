using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace iHentai.Common.Extensions
{
    class LoadingExtension
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.RegisterAttached(
            "ItemsSource", typeof(object), typeof(LoadingExtension), new PropertyMetadata(default(object)));

        public static void SetItemsSource(DependencyObject element, object value)
        {
            if (!(element is ProgressRing progressRing))
            {
                return;
            }
            
            static void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
            
            }


            if (GetItemsSource(element) is INotifyPropertyChanged oldItem)
            {
                oldItem.PropertyChanged -= OnPropertyChanged;
            }

            element.SetValue(ItemsSourceProperty, value);
        }


        public static object GetItemsSource(DependencyObject element)
        {
            return (object) element.GetValue(ItemsSourceProperty);
        }
    }
}