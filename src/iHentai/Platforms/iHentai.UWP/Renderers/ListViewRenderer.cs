using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;
using Microsoft.Xaml.Interactivity;
using Xamarin.Forms.Platform.UWP;
using CompositionTarget = Windows.UI.Xaml.Media.CompositionTarget;
using IListViewController = Xamarin.Forms.IListViewController;
using ListView = Xamarin.Forms.ListView;

//[assembly: ExportRenderer(typeof(ListView), typeof(iHentai.UWP.Renderers.ListViewRenderer))]
namespace iHentai.UWP.Renderers
{
    public class ListViewRenderer : Xamarin.Forms.Platform.UWP.ListViewRenderer
    {
        private PullToRefreshBehavior _behavior;

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
                if (List != null)
                {
                    _behavior = new PullToRefreshBehavior();
                    // Not working
                    var icon = new SymbolIcon(Symbol.Refresh)
                    {
                        Foreground = new SolidColorBrush(Colors.Black)
                    };
                    _behavior.IconElement = icon;
                    _behavior.RefreshRequested += Behavior_RefreshRequested;
                    Interaction.GetBehaviors(List).Add(_behavior);
                }
        }

        private void Behavior_RefreshRequested(object sender, RefreshRequestedEventArgs args)
        {
            IListViewController controller = Element;
            controller.SendRefreshing();
            //Not the right way
            args.GetDeferral().Complete();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            switch (e.PropertyName)
            {
                case nameof(ListView.IsRefreshing):
                    break;
                case nameof(ListView.IsPullToRefreshEnabled):
                case "RefreshAllowed":
                    //_behavior.Enabled = Element.IsPullToRefreshEnabled && (Element as Xamarin.Forms.IListViewController).RefreshAllowed;
                    break;
                default:
                    break;
            }
        }
    }

    public class RefreshRequestedEventArgs
    {
        internal RefreshRequestedEventArgs(DeferralCompletedHandler handler)
        {
            Handler = handler;
        }

        private DeferralCompletedHandler Handler { get; }
        private Deferral Deferral { get; set; }

        public Deferral GetDeferral()
        {
            return Deferral ?? (Deferral = new Deferral(Handler));
        }
    }

    public delegate void RefreshRequestedEventHandler(object sender, RefreshRequestedEventArgs args);

    public class AsyncDelegateCommand : ICommand
    {
        public AsyncDelegateCommand(Func<Task> execute, Func<bool> canExcute = null)
        {
            _execute = execute;
            _canExecute = canExcute ?? (() => true);
        }

        #region Events

        public event EventHandler CanExecuteChanged;

        #endregion

        #region Fields

        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;

        private bool _isExecuting;

        #endregion

        #region Methods

        public bool CanExecute(object parameter)
        {
            return !_isExecuting && _canExecute();
        }

        public bool CanExecute(object parameter, bool ignoreIsExecutingFlag)
        {
            if (ignoreIsExecutingFlag)
                return _canExecute();

            return !_isExecuting && _canExecute();
        }

        public async void Execute(object parameter)
        {
            _isExecuting = true;

            try
            {
                RaiseCanExecuteChanged();
                await _execute();
            }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        public async Task ExecuteAsync(object parameter)
        {
            _isExecuting = true;

            try
            {
                RaiseCanExecuteChanged();
                await _execute();
            }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }

    public class AsyncDelegateCommand<T> : ICommand
    {
        public AsyncDelegateCommand(Func<T, Task> execute, Func<T, bool> canExcute = null)
        {
            _execute = execute;
            _canExecute = canExcute ?? (x => true);
        }

        #region Events

        public event EventHandler CanExecuteChanged;

        #endregion

        #region Fields

        private readonly Func<T, Task> _execute;
        private readonly Func<T, bool> _canExecute;

        private bool _isExecuting;

        #endregion

        #region Methods

        public bool CanExecute(object parameter)
        {
            return !_isExecuting && _canExecute((T) parameter);
        }

        public bool CanExecute(object parameter, bool ignoreIsExecutingFlag)
        {
            if (ignoreIsExecutingFlag)
                return _canExecute((T) parameter);

            return !_isExecuting && _canExecute((T) parameter);
        }

        public async void Execute(object parameter)
        {
            _isExecuting = true;

            try
            {
                RaiseCanExecuteChanged();
                await _execute((T) parameter);
            }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        public async Task ExecuteAsync(object parameter)
        {
            _isExecuting = true;

            try
            {
                RaiseCanExecuteChanged();
                await _execute((T) parameter);
            }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }

    public static class Extensions
    {
        public static ScrollViewer GetScrollViewer(this DependencyObject element)
        {
            if (element is ScrollViewer scrollViewer)
                return scrollViewer;

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);

                var result = GetScrollViewer(child);
                if (result == null) continue;

                return result;
            }

            return null;
        }

        public static bool AlmostEqual(this float x, float y, float tolerance = 0.01f)
        {
            return Math.Abs(x - y) < tolerance;
        }
    }

    public class PullToRefreshBehavior : Behavior<ListViewBase>
    {
        #region Events

        public event RefreshRequestedEventHandler RefreshRequested;

        #endregion

        #region Fields

        private ScrollViewer _scrollViewer;
        private Compositor _compositor;

        private const float ScrollViewerMaxOverpanViewportRatio = 0.1f;

        private CompositionPropertySet _scrollerViewerManipulation;
        private ExpressionAnimation _rotationAnimation, _opacityAnimation, _offsetAnimation;
        private ScalarKeyFrameAnimation _resetAnimation, _loadingAnimation;

        private Visual _borderVisual;
        private Visual _refreshIconVisual;
        private float _refreshIconOffsetY;

        private bool _refresh;
        private DateTime _pulledDownTime, _restoredTime;

        private Task _pendingRefreshTask;
        private CancellationTokenSource _cts;

        private long _callbackId;

        #endregion

        #region Properties

        public FrameworkElement IconElement
        {
            get => (FrameworkElement) GetValue(IconElementProperty);
            set => SetValue(IconElementProperty, value);
        }

        public static readonly DependencyProperty IconElementProperty = DependencyProperty.Register("IconElement",
            typeof(FrameworkElement), typeof(PullToRefreshBehavior), new PropertyMetadata(null));

        public double IconElementMaxPulledDistance
        {
            get => (double) GetValue(IconElementMaxPulledDistanceProperty);
            set => SetValue(IconElementMaxPulledDistanceProperty, value);
        }

        public static readonly DependencyProperty IconElementMaxPulledDistanceProperty =
            DependencyProperty.Register("IconElementMaxPulledDistance", typeof(double), typeof(PullToRefreshBehavior),
                new PropertyMetadata(36.0d));

        public double IconElementMaxRotationAngle
        {
            get => (double) GetValue(IconElementMaxRotationAngleProperty);
            set => SetValue(IconElementMaxRotationAngleProperty, value);
        }

        public static readonly DependencyProperty IconElementMaxRotationAngleProperty =
            DependencyProperty.Register("IconElementMaxRotationAngle", typeof(double), typeof(PullToRefreshBehavior),
                new PropertyMetadata(400.0d));

        public double PullThresholdMaxOverpanRatio
        {
            get => (double) GetValue(PullThresholdMaxOverpanRatioProperty);
            set => SetValue(PullThresholdMaxOverpanRatioProperty, value);
        }

        public static readonly DependencyProperty PullThresholdMaxOverpanRatioProperty = DependencyProperty.Register(
            "PullThresholdMaxOverpanRatio", typeof(double), typeof(PullToRefreshBehavior), new PropertyMetadata(0.5d,
                (s, e) =>
                {
                    var threshold = (double) e.NewValue;
                    if (threshold <= 0 || threshold > 1)
                        throw new ArgumentOutOfRangeException(
                            $"{nameof(PullThresholdMaxOverpanRatio)} should be in between 0 and 1");
                }));

        public AsyncDelegateCommand<CancellationToken> RefreshCommand
        {
            get => (AsyncDelegateCommand<CancellationToken>) GetValue(RefreshCommandProperty);
            set => SetValue(RefreshCommandProperty, value);
        }

        public static readonly DependencyProperty RefreshCommandProperty = DependencyProperty.Register("RefreshCommand",
            typeof(AsyncDelegateCommand<CancellationToken>), typeof(PullToRefreshBehavior), new PropertyMetadata(null));

        public PullDirection PullDirection
        {
            get => (PullDirection) GetValue(PullDirectionProperty);
            set => SetValue(PullDirectionProperty, value);
        }

        public static readonly DependencyProperty PullDirectionProperty = DependencyProperty.Register("PullDirection",
            typeof(PullDirection), typeof(PullToRefreshBehavior), new PropertyMetadata(PullDirection.TopDown));

        private float IconElementMaxPulledOffsetY
            => (float) IconElementMaxPulledDistance * (PullDirection == PullDirection.TopDown ? 1 : -1);

        private string PullDistanceExpression =>
            $"max(0, {(PullDirection == PullDirection.TopDown ? string.Empty : "-")}ScrollManipulation.Translation.Y{(PullDirection == PullDirection.TopDown ? string.Empty : " -ScrollManipulation.ScrollableHeight")})";

        private string PullThresholdExpression =>
            $"(ScrollManipulation.MaxOverpan * {nameof(PullThresholdMaxOverpanRatio)})";

        #endregion

        #region Overrides

        /// <summary>
        ///     Called after the behavior is attached to the <see cref="Microsoft.Xaml.Interactivity.Behavior.AssociatedObject" />.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += OnAssociatedObjectLoaded;
            AssociatedObject.Unloaded += OnAssociatedObjectUnloaded;
        }

        /// <summary>
        ///     Called when the behavior is being detached from its
        ///     <see cref="Microsoft.Xaml.Interactivity.Behavior.AssociatedObject" />.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            _compositor?.Dispose();
            _scrollerViewerManipulation?.Dispose();
            _rotationAnimation?.Dispose();
            _opacityAnimation?.Dispose();
            _offsetAnimation?.Dispose();
            _resetAnimation?.Dispose();
            _loadingAnimation?.Dispose();
            _borderVisual?.Dispose();
            _refreshIconVisual?.Dispose();

            AssociatedObject.Loaded -= OnAssociatedObjectLoaded;
            AssociatedObject.Unloaded -= OnAssociatedObjectUnloaded;
        }

        #endregion

        #region Handlers

        private void OnAssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            _scrollViewer = AssociatedObject.GetScrollViewer();
            _scrollViewer.DirectManipulationStarted += OnScrollViewerDirectManipulationStarted;
            _scrollViewer.DirectManipulationCompleted += OnScrollViewerDirectManipulationCompleted;
            _callbackId = _scrollViewer.RegisterPropertyChangedCallback(ScrollViewer.ScrollableHeightProperty,
                OnScrollViewerScrollableHeightChanged);

            // Retrieve the ScrollViewer manipulation and the Compositor.
            _scrollerViewerManipulation =
                ElementCompositionPreview.GetScrollViewerManipulationPropertySet(_scrollViewer);
            _compositor = _scrollerViewerManipulation.Compositor;

            // Set boundaries.
            var listViewBaseVisual = ElementCompositionPreview.GetElementVisual(AssociatedObject);
            var clip = _compositor.CreateInsetClip();
            listViewBaseVisual.Clip = clip;

            // At the moment there are three things happening when pulling down the list -
            // 1. The Refresh Icon fades in.
            // 2. The Refresh Icon rotates (IconElementMaxRotationAngle).
            // 3. The Refresh Icon gets pulled down/up a bit (IconElementMaxOffsetY)
            // QUESTION 5
            // Can we also have Geometric Path animation so we can also draw the Refresh Icon along the way?
            //

            UpdateScrollableHeightInScrollViewerPropertySet();

            // Create a rotation expression animation based on the overpan distance of the ScrollViewer.
            _rotationAnimation =
                _compositor.CreateExpressionAnimation($"min({PullDistanceExpression} * DegreeMultiplier, MaxDegree)");
            _rotationAnimation.SetScalarParameter("DegreeMultiplier", 10.0f);
            _rotationAnimation.SetScalarParameter("MaxDegree", (float) IconElementMaxRotationAngle);
            _rotationAnimation.SetReferenceParameter("ScrollManipulation", _scrollerViewerManipulation);

            // Create an opacity expression animation based on the overpan distance of the ScrollViewer.
            _opacityAnimation =
                _compositor.CreateExpressionAnimation($"min({PullDistanceExpression} / {PullThresholdExpression}, 1)");
            _opacityAnimation.SetScalarParameter("PullThresholdMaxOverpanRatio", (float) PullThresholdMaxOverpanRatio);
            _opacityAnimation.SetReferenceParameter("ScrollManipulation", _scrollerViewerManipulation);

            // Create an offset expression animation based on the overpan distance of the ScrollViewer.
            _offsetAnimation =
                _compositor.CreateExpressionAnimation(
                    $"min({PullDistanceExpression} / {PullThresholdExpression}, 1) * MaxPulledDistance");
            _offsetAnimation.SetScalarParameter("PullThresholdMaxOverpanRatio", (float) PullThresholdMaxOverpanRatio);
            _offsetAnimation.SetScalarParameter("MaxPulledDistance", IconElementMaxPulledOffsetY);
            _offsetAnimation.SetReferenceParameter("ScrollManipulation", _scrollerViewerManipulation);

            // Create a keyframe animation to reset properties like Offset.Y, Opacity, etc.
            _resetAnimation = _compositor.CreateScalarKeyFrameAnimation();
            _resetAnimation.InsertKeyFrame(1.0f, 0.0f);

            // Create a loading keyframe animation (in this case, a rotation animation). 
            _loadingAnimation = _compositor.CreateScalarKeyFrameAnimation();
            _loadingAnimation.InsertExpressionKeyFrame(0.0f, "this.StartingValue");
            _loadingAnimation.InsertExpressionKeyFrame(1.0f, "this.StartingValue + 360");
            _loadingAnimation.Duration = TimeSpan.FromMilliseconds(1200);
            _loadingAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

            // Get the RefreshIcon's Visual.
            _refreshIconVisual = ElementCompositionPreview.GetElementVisual(IconElement);
            // Set the center point for the rotation animation.
            _refreshIconVisual.CenterPoint = new Vector3(IconElement.RenderSize.ToVector2() / 2, 0.0f);

            // Get the ListView's inner Border's Visual.
            var border = (Border) VisualTreeHelper.GetChild(AssociatedObject, 0);
            _borderVisual = ElementCompositionPreview.GetElementVisual(border);

            StartExpressionAnimations();
        }

        private void OnAssociatedObjectUnloaded(object sender, RoutedEventArgs e)
        {
            _scrollViewer.UnregisterPropertyChangedCallback(ScrollViewer.ScrollableHeightProperty, _callbackId);
            _scrollViewer.DirectManipulationStarted -= OnScrollViewerDirectManipulationStarted;
            _scrollViewer.DirectManipulationCompleted -= OnScrollViewerDirectManipulationCompleted;
        }

        private void OnScrollViewerScrollableHeightChanged(DependencyObject sender, DependencyProperty dp)
        {
            UpdateScrollableHeightInScrollViewerPropertySet();
        }

        private void OnScrollViewerDirectManipulationStarted(object sender, object e)
        {
            // QUESTION 1
            // I cannot think of a better way to monitor overpan changes, maybe there should be an Animating event?
            //
            CompositionTarget.Rendering += OnCompositionTargetRendering;

            // Initialise the values.
            _refresh = false;
        }

        private async void OnScrollViewerDirectManipulationCompleted(object sender, object e)
        {
            CompositionTarget.Rendering -= OnCompositionTargetRendering;

            //Debug.WriteLine($"ScrollViewer Rollback animation duration: {(_restoredTime - _pulledDownTime).Milliseconds}");

            // The ScrollViewer's rollback animation is appx. 200ms. So if the duration between the two DateTimes we recorded earlier
            // is greater than 250ms, we should cancel the refresh.
            var cancelled = _restoredTime - _pulledDownTime > TimeSpan.FromMilliseconds(250);

            if (!_refresh) return;

            if (cancelled)
            {
                Debug.WriteLine("Refresh cancelled...");
                StartResetAnimations();
            }
            else
            {
                Debug.WriteLine("Refresh now!!!");
                await StartLoadingAnimationAndRequestRefreshAsync(StartResetAnimations);
            }
        }

        private void OnCompositionTargetRendering(object sender, object e)
        {
            // QUESTION 2
            // What I've noticed is that I have to manually stop and
            // start the animation otherwise the Offset.Y is 0. Why?
            //
            _refreshIconVisual.StopAnimation("Offset.Y");

            // QUESTION 3
            // Why is the Translation always (0,0,0)?
            //
            //Vector3 translation;
            //var translationStatus = _scrollerViewerManipulation.TryGetVector3("Translation", out translation);
            //switch (translationStatus)
            //{
            //    case CompositionGetValueStatus.Succeeded:
            //        Debug.WriteLine($"ScrollViewer's Translation Y: {translation.Y}");
            //        break;
            //    case CompositionGetValueStatus.TypeMismatch:
            //    case CompositionGetValueStatus.NotFound:
            //    default:
            //        break;
            //}

            _refreshIconOffsetY = _refreshIconVisual.Offset.Y;
            //Debug.WriteLine($"RefreshIcon's Offset Y: {_refreshIconOffsetY}");

            // Question 4
            // It's not always the case here as the user can pull it all the way down and then push it back up to
            // CANCEL a refresh!! Though I cannot seem to find an easy way to detect right after the finger is lifted.
            // DirectManipulationCompleted is called too late.
            // What might be really helpful is to have a DirectManipulationDelta event with velocity and other values.
            //
            // At the moment I am calculating the time difference between the list gets pulled all the way down and rolled back up.
            // 
            if (!_refresh)
                _refresh = _refreshIconOffsetY.AlmostEqual(IconElementMaxPulledOffsetY);

            if (_refreshIconOffsetY.AlmostEqual(IconElementMaxPulledOffsetY))
            {
                _pulledDownTime = DateTime.Now;
                //Debug.WriteLine($"When the list is pulled down: {_pulledDownTime}");

                // Stop the Opacity animation on the RefreshIcon and the Offset.Y animation on the Border (ScrollViewer's host)
                _refreshIconVisual.StopAnimation("Opacity");
                _borderVisual.StopAnimation("Offset.Y");
            }

            if (_refresh && _refreshIconOffsetY <= 1)
                _restoredTime = DateTime.Now;

            _refreshIconVisual.StartAnimation("Offset.Y", _offsetAnimation);
        }

        #endregion

        #region Methods

        private void StartExpressionAnimations()
        {
            _refreshIconVisual.StartAnimation("RotationAngleInDegrees", _rotationAnimation);
            _refreshIconVisual.StartAnimation("Opacity", _opacityAnimation);
            _refreshIconVisual.StartAnimation("Offset.Y", _offsetAnimation);
            _borderVisual.StartAnimation("Offset.Y", _offsetAnimation);
        }

        private void StopExpressionAnimations()
        {
            _refreshIconVisual.StopAnimation("RotationAngleInDegrees");
            _refreshIconVisual.StopAnimation("Opacity");
            _refreshIconVisual.StopAnimation("Offset.Y");
            _borderVisual.StopAnimation("Offset.Y");
        }

        private async Task StartLoadingAnimationAndRequestRefreshAsync(Action completed)
        {
            // Create a short delay to allow the expression rotation animation to smoothly transition
            // to the new keyframe animation.
            await Task.Delay(100);

            _refreshIconVisual.StartAnimation("RotationAngleInDegrees", _loadingAnimation);

            // When using the event...
            if (RefreshRequested != null)
            {
                var e = new RefreshRequestedEventArgs(new DeferralCompletedHandler(completed));
                RefreshRequested.Invoke(this, e);
            }

            // When using the command...
            if (RefreshCommand != null)
            {
                try
                {
                    _cts?.Cancel();
                    _cts = new CancellationTokenSource();

                    _pendingRefreshTask = DoCancellableRefreshTaskAsync(_cts.Token);
                    await _pendingRefreshTask;
                }
                catch (OperationCanceledException)
                {
                }

                if (_cts != null && !_cts.IsCancellationRequested)
                    completed();
            }
        }

        private async Task DoCancellableRefreshTaskAsync(CancellationToken token)
        {
            try
            {
                if (_pendingRefreshTask != null)
                    await _pendingRefreshTask;
            }
            catch (OperationCanceledException)
            {
            }

            token.ThrowIfCancellationRequested();

            if (RefreshCommand != null && RefreshCommand.CanExecute(token, true))
                await RefreshCommand.ExecuteAsync(token);
        }

        private void StartResetAnimations()
        {
            StopExpressionAnimations();

            var batch = _compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
            // Looks like expression aniamtions will be removed after the following keyframe
            // animations have run. So here I have to re-start them once the keyframe animations
            // are completed.
            batch.Completed += (s, e) => StartExpressionAnimations();

            _borderVisual.StartAnimation("Offset.Y", _resetAnimation);
            _refreshIconVisual.StartAnimation("Opacity", _resetAnimation);

            batch.End();
        }

        private void UpdateScrollableHeightInScrollViewerPropertySet()
        {
            _scrollerViewerManipulation.InsertScalar("MaxOverpan",
                (float) _scrollViewer.ViewportHeight * ScrollViewerMaxOverpanViewportRatio);

            if (PullDirection == PullDirection.BottomUp)
                _scrollerViewerManipulation.InsertScalar("ScrollableHeight", (float) _scrollViewer.ScrollableHeight);
        }

        #endregion
    }

    public enum PullDirection
    {
        TopDown,
        BottomUp
    }
}