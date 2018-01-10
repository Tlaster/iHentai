using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json.Linq;

namespace Conet.Apis.Core
{
    public class Json : DependencyObject
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.RegisterAttached(
                "Text",
                typeof(string),
                typeof(Json),
                new PropertyMetadata(default)
            );
        
        public static void SetText(UIElement element, string value)
        {
            if (element is FrameworkElement frameworkElement)
            {
                var str = (frameworkElement.DataContext as JToken)?.SelectToken(value)?.Value<string>();
                if (str != null)
                {
                    switch (frameworkElement)
                    {
                        case TextBlock textBlock:
                            textBlock.Text = str;
                            break;
                        case TextBox textBox:
                            textBox.Text = str;
                            break;
                    }
                }
            }
            element.SetValue(TextProperty, value);
        }

        public static string GetText(UIElement element)
        {
            return (string) element.GetValue(TextProperty);
        }
    }
}