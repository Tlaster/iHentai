using System.ComponentModel;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace iHentai.Basic.Extensions
{
    public class FrameworkElementExtensions
    {
        /// <summary>
        ///     ClipToBounds Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty ClipToBoundsProperty =
            DependencyProperty.RegisterAttached(
                "ClipToBounds",
                typeof(bool),
                typeof(FrameworkElementExtensions),
                new PropertyMetadata(false, OnClipToBoundsChanged));

        /// <summary>
        ///     Gets the ClipToBounds property. This dependency property
        ///     indicates whether the element should be clipped to its bounds.
        /// </summary>
        public static bool GetClipToBounds(DependencyObject d)
        {
            return (bool) d.GetValue(ClipToBoundsProperty);
        }

        /// <summary>
        ///     Sets the ClipToBounds property. This dependency property
        ///     indicates whether the element should be clipped to its bounds.
        /// </summary>
        public static void SetClipToBounds(DependencyObject d, bool value)
        {
            d.SetValue(ClipToBoundsProperty, value);
        }

        /// <summary>
        ///     Handles changes to the ClipToBounds property.
        /// </summary>
        /// <param name="d">
        ///     The <see cref="DependencyObject" /> on which
        ///     the property has changed value.
        /// </param>
        /// <param name="e">
        ///     Event data that is issued by any event that
        ///     tracks changes to the effective value of this property.
        /// </param>
        private static void OnClipToBoundsChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var oldClipToBounds = (bool) e.OldValue;
            var newClipToBounds = (bool) d.GetValue(ClipToBoundsProperty);

            if (newClipToBounds)
                SetClipToBoundsHandler(d, new ClipToBoundsHandler());
            else
                SetClipToBoundsHandler(d, null);
        }

        /// <summary>
        ///     ClipToBoundsHandler Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty ClipToBoundsHandlerProperty =
            DependencyProperty.RegisterAttached(
                "ClipToBoundsHandler",
                typeof(ClipToBoundsHandler),
                typeof(FrameworkElementExtensions),
                new PropertyMetadata(null, OnClipToBoundsHandlerChanged));

        /// <summary>
        ///     Gets the ClipToBoundsHandler property. This dependency property
        ///     indicates the handler that handles the updates to the clipping geometry when ClipToBounds is set to true.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static ClipToBoundsHandler GetClipToBoundsHandler(DependencyObject d)
        {
            return (ClipToBoundsHandler) d.GetValue(ClipToBoundsHandlerProperty);
        }

        /// <summary>
        ///     Sets the ClipToBoundsHandler property. This dependency property
        ///     indicates the handler that handles the updates to the clipping geometry when ClipToBounds is set to true.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void SetClipToBoundsHandler(DependencyObject d, ClipToBoundsHandler value)
        {
            d.SetValue(ClipToBoundsHandlerProperty, value);
        }

        /// <summary>
        ///     Handles changes to the ClipToBoundsHandler property.
        /// </summary>
        /// <param name="d">
        ///     The <see cref="DependencyObject" /> on which
        ///     the property has changed value.
        /// </param>
        /// <param name="e">
        ///     Event data that is issued by any event that
        ///     tracks changes to the effective value of this property.
        /// </param>
        private static void OnClipToBoundsHandlerChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var oldClipToBoundsHandler = (ClipToBoundsHandler) e.OldValue;
            var newClipToBoundsHandler = (ClipToBoundsHandler) d.GetValue(ClipToBoundsHandlerProperty);

            oldClipToBoundsHandler?.Detach();
            newClipToBoundsHandler?.Attach((FrameworkElement) d);
        }

    }

    public class ClipToBoundsHandler
    {
        private FrameworkElement _fe;

        /// <summary>
        ///     Attaches to the specified framework element.
        /// </summary>
        /// <param name="fe">The fe.</param>
        public void Attach(FrameworkElement fe)
        {
            _fe = fe;
            UpdateClipGeometry();
            fe.SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            if (_fe == null)
                return;

            UpdateClipGeometry();
        }

        private void UpdateClipGeometry()
        {
            _fe.Clip =
                new RectangleGeometry
                {
                    Rect = new Rect(0, 0, _fe.ActualWidth, _fe.ActualHeight)
                };
        }

        /// <summary>
        ///     Detaches this instance.
        /// </summary>
        public void Detach()
        {
            if (_fe == null)
                return;

            _fe.SizeChanged -= OnSizeChanged;
            _fe = null;
        }
    }
}