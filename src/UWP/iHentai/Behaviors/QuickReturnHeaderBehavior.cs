using System;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Marduk.Controls;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Animations.Behaviors;
using Microsoft.Toolkit.Uwp.UI.Animations.Expressions;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using ListViewBase = Windows.UI.Xaml.Controls.ListViewBase;

namespace iHentai.Behaviors
{
    public class QuickReturnHeaderBehavior : BehaviorBase<FrameworkElement>
    {
        /// <summary>
        ///     The UIElement that will be faded.
        /// </summary>
        public static readonly DependencyProperty HeaderElementProperty = DependencyProperty.Register(
            nameof(HeaderElement), typeof(UIElement), typeof(QuickReturnHeaderBehavior),
            new PropertyMetadata(null, PropertyChangedCallback));

        //private CompositionPropertySet _animationProperties;
        private double _headerPosition;
        //private Visual _headerVisual;
        //private CompositionPropertySet _scrollProperties;

        private ScrollViewer _scrollViewer;

        /// <summary>
        ///     Gets or sets the target element for the ScrollHeader behavior.
        /// </summary>
        /// <remarks>
        ///     Set this using the header of a ListView or GridView.
        /// </remarks>
        public UIElement HeaderElement
        {
            get => (UIElement) GetValue(HeaderElementProperty);
            set => SetValue(HeaderElementProperty, value);
        }

        /// <summary>
        ///     Attaches the behavior to the associated object.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if attaching succeeded; otherwise <c>false</c>.
        /// </returns>
        protected override bool Initialize()
        {
            var result = AssignAnimation();
            return result;
        }

        /// <summary>
        ///     Detaches the behavior from the associated object.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if detaching succeeded; otherwise <c>false</c>.
        /// </returns>
        protected override bool Uninitialize()
        {
            RemoveAnimation();
            return true;
        }

        /// <summary>
        ///     Show the header
        /// </summary>
        public void Show()
        {
            //if (_headerVisual != null && _scrollViewer != null)
            //    _animationProperties.InsertScalar("OffsetY", 0.0f);
        }

        /// <summary>
        ///     If any of the properties are changed then the animation is automatically started depending on the QuickReturn and
        ///     IsSticky properties.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var b = d as QuickReturnHeaderBehavior;
            b?.AssignAnimation();
        }

        /// <summary>
        ///     Uses Composition API to get the UIElement and sets an ExpressionAnimation
        ///     The ExpressionAnimation uses the height of the UIElement to calculate an opacity value
        ///     for the Header as it is scrolling off-screen. The opacity reaches 0 when the Header
        ///     is entirely scrolled off.
        /// </summary>
        /// <returns><c>true</c> if the assignment was successful; otherwise, <c>false</c>.</returns>
        private bool AssignAnimation()
        {
            StopAnimation();

            // Confirm that Windows.UI.Xaml.Hosting.ElementCompositionPreview is available (Windows 10 10586 or later).
            if (!ApiInformation.IsMethodPresent("Windows.UI.Xaml.Hosting.ElementCompositionPreview",
                nameof(ElementCompositionPreview.GetScrollViewerManipulationPropertySet)))
                return true;

            if (AssociatedObject == null)
                return false;

            if (_scrollViewer == null)
                _scrollViewer = AssociatedObject as ScrollViewer ?? AssociatedObject.FindDescendant<ScrollViewer>();

            if (_scrollViewer == null)
                return false;

            var waterfall = AssociatedObject as Marduk.Controls.VirtualizingPanel ??
                            AssociatedObject.FindDescendant<Marduk.Controls.VirtualizingPanel>();
            var listView = AssociatedObject as ListViewBase ?? AssociatedObject.FindDescendant<ListViewBase>();

            if (listView?.ItemsPanelRoot != null)
                Canvas.SetZIndex(listView.ItemsPanelRoot, -1);

            //if (_scrollProperties == null)
            //    _scrollProperties = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(_scrollViewer);

            //if (_scrollProperties == null)
            //    return false;

            // Implicit operation: Find the Header object of the control if it uses ListViewBase
            if (HeaderElement == null)
            {
                if (listView != null)
                    HeaderElement = listView.Header as UIElement;
                else if (waterfall != null)
                {
                    //HeaderElement = waterfall.Header as UIElement;
                }
            }
            if (!(HeaderElement is FrameworkElement headerElement) || headerElement.RenderSize.Height == 0)
                return false;

            //Canvas.SetZIndex(HeaderElement, short.MaxValue - 1);
            //if (_headerVisual == null)
            //    _headerVisual = ElementCompositionPreview.GetElementVisual(headerElement);

            //if (_headerVisual == null)
            //    return false;

            _scrollViewer.ViewChanged -= ScrollViewer_ViewChanged;
            _scrollViewer.ViewChanged += ScrollViewer_ViewChanged;

            //_scrollViewer.GotFocus -= ScrollViewer_GotFocus;
            //_scrollViewer.GotFocus += ScrollViewer_GotFocus;
            headerElement.SizeChanged -= ScrollHeader_SizeChanged;
            headerElement.SizeChanged += ScrollHeader_SizeChanged;

            //var compositor = _scrollProperties.Compositor;

            //if (_animationProperties == null)
            //{
            //    _animationProperties = compositor.CreatePropertySet();
            //    _animationProperties.InsertScalar("OffsetY", 0.0f);
            //}

            //var propSetOffset = _animationProperties.GetReference().GetScalarProperty("OffsetY");
            //var scrollPropSet = _scrollProperties.GetSpecializedReference<ManipulationPropertySetReferenceNode>();
            //var expressionAnimation =
            //    ExpressionFunctions.Max(ExpressionFunctions.Min(propSetOffset, -scrollPropSet.Translation.Y), 0);

            //_headerVisual.StartAnimation("Offset.Y", expressionAnimation);

            return true;
        }

        /// <summary>
        ///     Remove the animation from the UIElement.
        /// </summary>
        private void RemoveAnimation()
        {
            if (_scrollViewer != null)
            {
                _scrollViewer.ViewChanged -= ScrollViewer_ViewChanged;
                //_scrollViewer.GotFocus -= ScrollViewer_GotFocus;
            }

            if (HeaderElement is FrameworkElement element)
                element.SizeChanged -= ScrollHeader_SizeChanged;

            StopAnimation();
        }

        /// <summary>
        ///     Stop the animation of the UIElement.
        /// </summary>
        private void StopAnimation()
        {
            //_headerVisual?.StopAnimation("Offset.Y");

            ////_animationProperties?.InsertScalar("OffsetY", 0.0f);

            //if (_headerVisual != null)
            //{
            //    var offset = _headerVisual.Offset;
            //    offset.Y = 0.0f;
            //    _headerVisual.Offset = offset;
            //}
        }

        private void ScrollHeader_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AssignAnimation();
        }

        private double _prevVerticalOffset;
        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            //if (_animationProperties != null)
            //{
                var header = (FrameworkElement) HeaderElement;
                var headerHeight = header.ActualHeight;
                var changed = _prevVerticalOffset - _scrollViewer.VerticalOffset;
                var res = _headerPosition + changed;
                if (changed > 0)
                {
                    _headerPosition = Math.Min(0d, res);
                    header.Offset(offsetY: (float)_headerPosition, duration: 0).Start();
                    //_animationProperties.InsertScalar("OffsetY", (float)Math.Min(0d, res));
                }
                else if (changed < 0)
                {

                    _headerPosition = Math.Max(-headerHeight, res);
                    header.Offset(offsetY: (float)_headerPosition, duration: 0).Start();
                    //_animationProperties.InsertScalar("OffsetY", (float)Math.Max(-headerHeight, res));
                }
                _prevVerticalOffset = _scrollViewer.VerticalOffset;
            //}
        }

        private void ScrollViewer_GotFocus(object sender, RoutedEventArgs e)
        {
            var scroller = (ScrollViewer) sender;

            var focusedElement = FocusManager.GetFocusedElement();

            if (focusedElement is UIElement element)
            {
                var header = (FrameworkElement) HeaderElement;

                var point = element.TransformToVisual(scroller).TransformPoint(new Point(0, 0));

                if (point.Y < header.ActualHeight)
                    scroller.ChangeView(0, scroller.VerticalOffset - (header.ActualHeight - point.Y), 1, false);
            }
        }
    }
}