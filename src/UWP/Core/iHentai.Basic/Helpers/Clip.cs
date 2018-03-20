using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace iHentai.Basic.Helpers
{
    public class Clip
    {
        /// <summary>
        ///     Identifies the ToBounds Dependency Property.
        ///     <summary>
        public static readonly DependencyProperty ToBoundsProperty =
            DependencyProperty.RegisterAttached("ToBounds", typeof(bool),
                typeof(Clip), new PropertyMetadata(false, OnToBoundsPropertyChanged));

        public static bool GetToBounds(DependencyObject depObj)
        {
            return (bool) depObj.GetValue(ToBoundsProperty);
        }

        public static void SetToBounds(DependencyObject depObj, bool clipToBounds)
        {
            depObj.SetValue(ToBoundsProperty, clipToBounds);
        }

        private static void OnToBoundsPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement fe)
            {
                ClipToBounds(fe);

                // whenever the element which this property is attached to is loaded
                // or re-sizes, we need to update its clipping geometry
                fe.Loaded += fe_Loaded;
                fe.SizeChanged += fe_SizeChanged;
            }
        }

        /// <summary>
        ///     Creates a rectangular clipping geometry which matches the geometry of the
        ///     passed element
        /// </summary>
        private static void ClipToBounds(FrameworkElement fe)
        {
            if (GetToBounds(fe))
                fe.Clip = new RectangleGeometry
                {
                    Rect = new Rect(0, 0, fe.ActualWidth, fe.ActualHeight)
                };
            else
                fe.Clip = null;
        }

        private static void fe_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ClipToBounds(sender as FrameworkElement);
        }

        private static void fe_Loaded(object sender, RoutedEventArgs e)
        {
            ClipToBounds(sender as FrameworkElement);
        }
    }
}