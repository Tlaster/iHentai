using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace iHentai.Controls.Paging
{
    public class ActivityContainer : Control, INavigate
    {
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            nameof(Content), typeof(object), typeof(ActivityContainer), new PropertyMetadata(default));

        public static readonly DependencyProperty ContentTransitionsProperty = DependencyProperty.Register(
            nameof(ContentTransitions), typeof(TransitionCollection), typeof(ActivityContainer),
            new PropertyMetadata(default, OnContentTransitionsChanged));

        public static readonly DependencyProperty SourceActivityTypeProperty = DependencyProperty.Register(
            nameof(SourceActivityType), typeof(Type), typeof(ActivityContainer),
            new PropertyMetadata(default, OnSourceActivityTypeChanged));

        private readonly ActivityStackManager _activityStackManager = new ActivityStackManager();

        public ActivityContainer()
        {
            HorizontalContentAlignment = HorizontalAlignment.Stretch;
            VerticalContentAlignment = VerticalAlignment.Stretch;

            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;

            Loaded += delegate { Window.Current.VisibilityChanged += OnVisibilityChanged; };
            Unloaded += delegate { Window.Current.VisibilityChanged -= OnVisibilityChanged; };
            _activityStackManager.BackStackChanged += ActivityStackManagerOnBackStackChanged;
            DefaultStyleKey = typeof(ActivityContainer);
        }

        private void ActivityStackManagerOnBackStackChanged()
        {
            BackStackChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool DisableCache { get; set; }

        public ContentPresenter InternalContentPresenter { get; private set; }

        private ActivityModel CurrentActivityModel => _activityStackManager?.CurrentActivity;

        public Activity CurrentActivity => _activityStackManager?.CurrentActivity?.GetActivity(this);

        public bool CanGoBack => _activityStackManager.CanGoBack;

        public int BackStackDepth => _activityStackManager.BackStackDepth;

        public bool IsNavigating { get; private set; }

        public IActivityTransition ActivityTransition { get; set; }

        private IActivityTransition ActualActivityTransition
        {
            get
            {
                if (ContentTransitions != null)
                    return null;

                var currentActivity = CurrentActivity;
                return currentActivity?.ActivityTransition != null
                    ? CurrentActivity.ActivityTransition
                    : ActivityTransition;
            }
        }

        private Grid ContentRoot
        {
            get
            {
                if (Content == null)
                    Content = new Grid();
                return (Grid) Content;
            }
        }

        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public TransitionCollection ContentTransitions
        {
            get => (TransitionCollection) GetValue(ContentTransitionsProperty);
            set => SetValue(ContentTransitionsProperty, value);
        }

        public Type SourceActivityType
        {
            get => (Type) GetValue(SourceActivityTypeProperty);
            set => SetValue(SourceActivityTypeProperty, value);
        }

        public bool Navigate(Type sourceActivityType)
        {
            Navigate(sourceActivityType, null);
            return true;
        }

        private static void OnContentTransitionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ActivityContainer).OnContentTransitionsChanged(e.NewValue);
        }

        private void OnContentTransitionsChanged(object newValue)
        {
            ContentRoot.ChildrenTransitions = newValue as TransitionCollection;
        }

        public event EventHandler<EventArgs> Navigated;
        public event EventHandler<EventArgs> Navigating;
        public event EventHandler BackStackChanged;

        public Task<bool> GoHome()
        {
            return GoBackTo(0);
        }

        public async Task<bool> GoBackTo(int index)
        {
            if (!_activityStackManager.CanGoBackTo(index)) return false;

            return await RunNavigationWithCheck(async () =>
            {
                var nextActivity = _activityStackManager.GetActivityAt(index);
                var currentActivity = CurrentActivityModel;


                await NavigateImplAsync(NavigationMode.Back, currentActivity, nextActivity, index);

                _activityStackManager.ClearForwardStack();

                return true;
            });
        }

        public bool RemoveActivityAt(int index)
        {
            return _activityStackManager.RemoveActivityAt(index);
        }

        public Task<bool> GoBack()
        {
            return RunNavigationWithCheck(async () =>
            {
                await GoForwardOrBack(NavigationMode.Back);
                return true;
            });
        }

        public void Initialize(Type homeActivityType, object parameter = null, Dictionary<string, object> intent = null)
        {
            Navigate(homeActivityType, parameter, intent);
        }
        
        public Task<bool> Navigate(Type activityType, object parameter, Dictionary<string, object> intent = null)
        {
            var newActivity = new ActivityModel(activityType, parameter, intent);
            return NavigateWithMode(newActivity, NavigationMode.New);
        }

        public Task<bool> Navigate<T>(object parameter = null, Dictionary<string, object> intent = null) where T: Activity
        {
            return Navigate(typeof(T), parameter, intent);
        }

        private static void OnSourceActivityTypeChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            (dependencyObject as ActivityContainer)?.OnSourceActivityTypeChanged(e.NewValue as Type);
        }

        private void OnSourceActivityTypeChanged(Type newValue)
        {
            Navigate(newValue);
        }

        public void ClearBackStack()
        {
            _activityStackManager.ClearBackStack();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InternalContentPresenter = (ContentPresenter) GetTemplateChild(nameof(ActivityContainer));
        }

        private Task<bool> NavigateWithMode(ActivityModel newActivity, NavigationMode navigationMode)
        {
            return RunNavigationWithCheck(async () =>
            {
                var currentActivity = CurrentActivityModel;

                _activityStackManager.ClearForwardStack();

                await NavigateImplAsync(navigationMode, currentActivity, newActivity,
                    _activityStackManager.CurrentIndex + 1);

                return true;
            });
        }

        private async Task<bool> RunNavigationWithCheck(Func<Task<bool>> task)
        {
            if (IsNavigating) return false;

            try
            {
                IsNavigating = true;
                return await task();
            }
            finally
            {
                IsNavigating = false;
            }
        }


        private async Task GoForwardOrBack(NavigationMode navigationMode)
        {
            if (CanGoBack && !CurrentActivity.OnBackRequest())
            {
                var currentActivity = CurrentActivityModel;
                var nextActivityIndex =
                    _activityStackManager.CurrentIndex - 1;
                var nextActivity = _activityStackManager.Activities[nextActivityIndex];

                await NavigateImplAsync(navigationMode, currentActivity, nextActivity, nextActivityIndex);

                _activityStackManager.ClearForwardStack();
            }
            else
            {
                throw new InvalidOperationException($"The {nameof(ActivityContainer)} cannot go back");
            }
        }

        private async Task NavigateImplAsync(NavigationMode navigationMode,
            ActivityModel currentActivity, ActivityModel nextActivity, int nextIndex)
        {
            ContentRoot.IsHitTestVisible = false;

            InvokeLifecycleBeforeContentChanged(navigationMode, currentActivity, nextActivity);

            _activityStackManager.ChangeCurrentActivity(nextActivity, nextIndex);

            OnCurrentActivityChanged(currentActivity?.Activity, nextActivity?.Activity);

            Navigating?.Invoke(this, EventArgs.Empty);

            await UpdateContent(navigationMode, currentActivity, nextActivity);

            InvokeLifecycleAfterContentChanged(navigationMode, currentActivity, nextActivity);

            ContentRoot.IsHitTestVisible = true;

            ReleaseActivity(currentActivity);

            Navigated?.Invoke(this, EventArgs.Empty);
        }

        private void AddActivityToContentRoot(NavigationMode navigationMode, ActivityInsertionMode insertionMode,
            FrameworkElement next)
        {
            switch (navigationMode)
            {
                case NavigationMode.New when insertionMode == ActivityInsertionMode.NewAbove:
                case NavigationMode.Back when insertionMode == ActivityInsertionMode.NewBelow:
                    ContentRoot.Children.Add(next);
                    break;
                case NavigationMode.Back when insertionMode == ActivityInsertionMode.NewAbove:
                case NavigationMode.New when insertionMode == ActivityInsertionMode.NewBelow:
                    ContentRoot.Children.Insert(0, next);
                    break;
            }
        }

        private async Task UpdateContent(NavigationMode navigationMode, ActivityModel currentActivity,
            ActivityModel nextActivity)
        {
            var animation = ActualActivityTransition;
            var current = currentActivity?.GetActivity(this)?.InternalPage;
            var next = nextActivity?.GetActivity(this)?.InternalPage;
            currentActivity?.GetActivity(this)?.PrepareConnectedAnimation();
            if (animation != null)
            {
                AddActivityToContentRoot(navigationMode, animation.InsertionMode, next);
                nextActivity?.GetActivity(this)?.UsingConnectedAnimation();
                switch (navigationMode)
                {
                    case NavigationMode.New:
                        await animation.OnStart(next, current);
                        break;
                    case NavigationMode.Back:
                        await animation.OnClose(current, next);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(navigationMode), navigationMode, null);
                }

                ContentRoot.Children.Remove(current);
            }
            else
            {
                if (ContentRoot.Children.Any())
                    ContentRoot.Children[0] = next;
                else
                    ContentRoot.Children.Add(next);
                nextActivity?.GetActivity(this)?.UsingConnectedAnimation();
            }
        }

        private void InvokeLifecycleBeforeContentChanged(NavigationMode navigationMode, ActivityModel currentActivity,
            ActivityModel nextActivity)
        {
            switch (navigationMode)
            {
                case NavigationMode.New:
                    currentActivity?.GetActivity(this)?.OnPause();
                    nextActivity?.GetActivity(this)?.OnStart();
                    break;
                case NavigationMode.Back:
                    currentActivity?.GetActivity(this)?.OnClose();
                    nextActivity?.GetActivity(this).OnRestart();
                    break;
                case NavigationMode.Forward:
                    break;
                case NavigationMode.Refresh:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(navigationMode), navigationMode, null);
            }
        }

        private void InvokeLifecycleAfterContentChanged(NavigationMode navigationMode, ActivityModel currentActivity,
            ActivityModel nextActivity)
        {
            switch (navigationMode)
            {
                case NavigationMode.New:
                    currentActivity?.GetActivity(this)?.OnStop();
                    nextActivity?.GetActivity(this)?.OnResume();
                    break;
                case NavigationMode.Back:
                    currentActivity?.GetActivity(this)?.OnDestroy();
                    nextActivity?.GetActivity(this).OnResume();
                    break;
                case NavigationMode.Forward:
                    break;
                case NavigationMode.Refresh:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(navigationMode), navigationMode, null);
            }
        }

        private void ReleaseActivity(ActivityModel activity)
        {
            if (activity != null &&
                (activity.Activity.NavigationCacheMode == NavigationCacheMode.Disabled || DisableCache))
                activity.Release();
        }

        protected virtual void OnCurrentActivityChanged(Activity currentActivity, Activity newActivity)
        {
        }

        private void OnVisibilityChanged(object sender, VisibilityChangedEventArgs args)
        {
            if (args.Visible)
                CurrentActivity?.OnResume();
            else
                CurrentActivity?.OnPause();
        }

        internal void FinishActivity(Activity activity)
        {
            if (CurrentActivity == activity)
            {
                if (CanGoBack)
                {
                    GoBack();
                }
            }
            else
            {
                _activityStackManager.RemoveActivity(activity);
            }
        }
    }
}