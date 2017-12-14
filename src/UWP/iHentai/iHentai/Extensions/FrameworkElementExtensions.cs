using System;
using Windows.UI.Xaml;

namespace iHentai.Extensions
{
    public static class FrameworkElementExtensions
    {
        public static void Post(this FrameworkElement element, Action action)
        {
            void Handler(object sender, RoutedEventArgs e)
            {
                element.Loaded -= Handler;
                action?.Invoke();
            }

            element.Loaded += Handler;
        }
    }
}