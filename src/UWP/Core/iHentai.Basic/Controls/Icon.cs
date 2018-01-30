using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace iHentai.Basic.Controls
{
    public partial class Icon : FontIcon
    {
        public static readonly DependencyProperty IconsProperty = DependencyProperty.Register(
            nameof(Icons), typeof(Icons), typeof(Icon), new PropertyMetadata(default(Icons), OnIconsChanged));

        private static void OnIconsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as Icon).OnIconsChanged((Icons) e.NewValue);
        }

        private void OnIconsChanged(Icons newValue)
        {
            Glyph = Mapper[newValue];
        }

        public Icon()
        {
            FontFamily = new FontFamily("Segoe MDL2 Assets");
        }

        public Icons Icons
        {
            get => (Icons) GetValue(IconsProperty);
            set => SetValue(IconsProperty, value);
        }
    }
}