using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using iHentai.Paging.Animations;
using iHentai.Paging.Handlers;

namespace iHentai.Paging
{
    public delegate void NavigatedEventHandler(object sender, HentaiNavigationEventArgs e);

    public delegate void NavigatingEventHandler(object sender, HentaiNavigatingCancelEventArgs e);

    public class HentaiFrame : Control, INavigate
    {
        private readonly PageStackManager _pageStackManager = new PageStackManager();


        public HentaiFrame()
        {
            AutomaticBackButtonHandling = true;

            HorizontalContentAlignment = HorizontalAlignment.Stretch;
            VerticalContentAlignment = VerticalAlignment.Stretch;

            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;

            Loaded += delegate { Window.Current.VisibilityChanged += OnVisibilityChanged; };
            Unloaded += delegate { Window.Current.VisibilityChanged -= OnVisibilityChanged; };

            //GoBackCommand = new AsyncRelayCommand(GoBackAsync, () => CanGoBack);

            DefaultStyleKey = typeof(HentaiFrame);

            DisableForwardStack = ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons");
        }


        public bool ShowNavigationOnAppInAndOut { get; set; }


        public bool DisableForwardStack { get; set; }


        public bool DisableCache { get; set; }


        public Frame InternalFrame { get; private set; }


        public static HentaiFrame Current => Window.Current.Content as HentaiFrame;
        

        public bool AutomaticBackButtonHandling
        {
            get => _pageStackManager.AutomaticBackButtonHandling;
            set => _pageStackManager.AutomaticBackButtonHandling = value;
        }

        public bool IsFirstPage => _pageStackManager.IsFirstPage;


        internal HentaiPageModel PreviousPage => _pageStackManager.PreviousPage;


        internal HentaiPageModel CurrentPage => _pageStackManager.CurrentPage;


        internal HentaiPageModel NextPage => _pageStackManager.NextPage;


        public bool CanGoBack => _pageStackManager.CanGoBack;


        public bool CanGoForward => _pageStackManager.CanGoForward;


        internal IReadOnlyList<HentaiPageModel> Pages => _pageStackManager.Pages;


        public int BackStackDepth => _pageStackManager.BackStackDepth;


        public IPageAnimation PageAnimation { get; set; }

        private IPageAnimation ActualPageAnimation
        {
            get
            {
                if (ContentTransitions != null)
                    return null;

                var currentPage = CurrentPage;
                return currentPage?.Page?.PageAnimation != null
                    ? CurrentPage.Page.PageAnimation
                    : PageAnimation;
            }
        }

        private Grid ContentGrid
        {
            get
            {
                if (Content == null)
                    Content = new Grid();
                return (Grid) Content;
            }
        }

        public bool IsNavigating { get; private set; }


        public bool Navigate(Type sourcePageType)
        {
            NavigateAsync(sourcePageType);
            return true;
        }


        public event NavigatedEventHandler Navigated;


        public event NavigatingEventHandler Navigating;


        public Task<bool> GoForwardAsync()
        {
            return RunNavigationWithCheckAsync(async () =>
            {
                if (await RaisePageOnNavigatingFromAsync(CurrentPage, null, NavigationMode.Forward))
                    return false;

                await GoForwardOrBackAsync(NavigationMode.Forward);
                return true;
            });
        }


        internal HentaiPageModel GetNearestPageOfTypeInBackStack(Type pageType)
        {
            return _pageStackManager.GetNearestPageOfTypeInBackStack(pageType);
        }


        internal async Task<bool> GoBackToAsync(HentaiPageModel pageDescription)
        {
            var index = _pageStackManager.GetPageIndex(pageDescription);
            return await GoBackToAsync(index);
        }


        public Task<bool> GoHomeAsync()
        {
            return GoBackToAsync(0);
        }


        public Task<bool> GoBackToAsync(int newPageIndex)
        {
            if (!_pageStackManager.CanGoBackTo(newPageIndex))
                return Task.FromResult(false);

            return RunNavigationWithCheckAsync(async () =>
            {
                var nextPage = _pageStackManager.GetPageAt(newPageIndex);
                var currentPage = CurrentPage;

                if (await RaisePageOnNavigatingFromAsync(CurrentPage, currentPage, NavigationMode.Back))
                    return false;

                await NavigateWithAnimationsAndCallbacksAsync(NavigationMode.Back, currentPage, nextPage, newPageIndex);

                if (DisableForwardStack)
                    _pageStackManager.ClearForwardStack();

                return true;
            });
        }


        internal bool RemovePageFromStack(HentaiPageModel pageDescription)
        {
            return _pageStackManager.RemovePageFromStack(pageDescription);
        }


        public bool RemovePageFromStackAt(int pageIndex)
        {
            return _pageStackManager.RemovePageFromStackAt(pageIndex);
        }


        public Task<bool> GoBackAsync()
        {
            return RunNavigationWithCheckAsync(async () =>
            {
                if (await RaisePageOnNavigatingFromAsync(CurrentPage, PreviousPage, NavigationMode.Back))
                    return false;

                await GoForwardOrBackAsync(NavigationMode.Back);
                return true;
            });
        }


        public void Initialize(Type homePageType, object parameter = null)
        {
            NavigateAsync(homePageType, parameter);
        }


        public async Task<bool> NavigateAsync(Type pageType, object parameter = null)
        {
            var newPage = new HentaiPageModel(pageType, parameter);
            return await NavigateAsync(newPage, NavigationMode.New);
        }


        public async Task<bool> NavigateToExistingOrNewPageAsync(Type pageType, object pageParameter = null)
        {
            var existingPage = GetNearestPageOfTypeInBackStack(pageType);
            if (existingPage != null)
            {
                existingPage.Parameter = pageParameter;
                return await CopyToTopAndNavigateAsync(existingPage);
            }
            return await NavigateAsync(pageType, pageParameter);
        }


        internal Task<bool> MoveToTopAndNavigateAsync(HentaiPageModel page)
        {
            return _pageStackManager.MoveToTop(page, async p => await NavigateAsync(p, NavigationMode.Forward));
        }


        internal async Task<bool> CopyToTopAndNavigateAsync(HentaiPageModel page)
        {
            if (CurrentPage == page)
                return true;

            if (_pageStackManager.Pages.Contains(page))
                if (await NavigateAsync(page, NavigationMode.Forward))
                    return true;

            return false;
        }


        public void ClearBackStack()
        {
            _pageStackManager.ClearBackStack();
        }

        protected virtual void OnPageCreated(object sender, object page)
        {
            // Must be empty. 
        }


        protected virtual void OnNavigated(object sender, HentaiNavigationEventArgs args)
        {
            // Must be empty. 
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InternalFrame = (Frame) GetTemplateChild("Frame");
        }

        private Task<bool> NavigateAsync(HentaiPageModel newPage, NavigationMode navigationMode)
        {
            return RunNavigationWithCheckAsync(async () =>
            {
                var currentPage = CurrentPage;
                if (currentPage != null)
                    if (await RaisePageOnNavigatingFromAsync(CurrentPage, newPage, navigationMode))
                        return false;

                _pageStackManager.ClearForwardStack();

                await NavigateWithAnimationsAndCallbacksAsync(navigationMode, currentPage, newPage,
                    _pageStackManager.CurrentIndex + 1);

                return true;
            });
        }

        private async Task<bool> RunNavigationWithCheckAsync(Func<Task<bool>> task)
        {
            if (IsNavigating)
                return false;

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


        private async Task GoForwardOrBackAsync(NavigationMode navigationMode)
        {
            if (navigationMode == NavigationMode.Forward ? CanGoForward : CanGoBack)
            {
                var currentPage = CurrentPage;
                var nextPageIndex =
                    _pageStackManager.CurrentIndex + (navigationMode == NavigationMode.Forward ? 1 : -1);
                var nextPage = _pageStackManager.Pages[nextPageIndex];

                await NavigateWithAnimationsAndCallbacksAsync(navigationMode, currentPage, nextPage, nextPageIndex);

                if (navigationMode == NavigationMode.Back && DisableForwardStack)
                    _pageStackManager.ClearForwardStack();
            }
            else
            {
                throw new InvalidOperationException("The frame cannot go forward or back");
            }
        }

        private async Task NavigateWithAnimationsAndCallbacksAsync(NavigationMode navigationMode,
            HentaiPageModel currentPage, HentaiPageModel nextPage, int nextPageIndex)
        {
            var pageAnimation = ActualPageAnimation;
            var insertionMode = pageAnimation?.PageInsertionMode ?? PageInsertionMode.Sequential;

            ContentGrid.IsHitTestVisible = false;

            AddNewPageToGridIfNotSequential(insertionMode, nextPage);

            await AnimateNavigatedFromIfCurrentPageNotNull(pageAnimation, navigationMode, insertionMode, currentPage,
                nextPage);

            SwitchPagesIfSequential(insertionMode, currentPage, nextPage);
            ChangeCurrentPageAndRaiseNavigationEvents(navigationMode, currentPage, nextPage, nextPageIndex);

            await AnimateNavigatedToAndRemoveCurrentPageAsync(pageAnimation, navigationMode, insertionMode, currentPage,
                nextPage);

            ContentGrid.IsHitTestVisible = true;

            ReleasePageIfNecessary(currentPage);
        }

        private void ReleasePageIfNecessary(HentaiPageModel page)
        {
            if (page != null && (page.Page.NavigationCacheMode == NavigationCacheMode.Disabled || DisableCache))
                page.ReleasePage();
        }

        private void AddNewPageToGridIfNotSequential(PageInsertionMode insertionMode, HentaiPageModel newPage)
        {
            var page = newPage.GetPage(this).InternalPage;

            if (!ContentGrid.Children.Contains(page))
                switch (insertionMode)
                {
                    case PageInsertionMode.ConcurrentAbove:
                        ContentGrid.Children.Add(page);
                        break;
                    case PageInsertionMode.ConcurrentBelow:
                        ContentGrid.Children.Insert(0, page);
                        break;
                }
        }

        private void ChangeCurrentPageAndRaiseNavigationEvents(NavigationMode navigationMode,
            HentaiPageModel currentPage,
            HentaiPageModel newPage, int nextPageIndex)
        {
            if (currentPage != null)
                RaisePageOnNavigatedFrom(currentPage, navigationMode);

            _pageStackManager.ChangeCurrentPage(newPage, nextPageIndex);

            RaisePageOnNavigatedTo(newPage, navigationMode);
            GoBackAsync();
        }

        private void SwitchPagesIfSequential(PageInsertionMode insertionMode, HentaiPageModel currentPage,
            HentaiPageModel newPage)
        {
            if (insertionMode == PageInsertionMode.Sequential)
            {
                if (currentPage != null)
                    ContentGrid.Children.Remove(currentPage.GetPage(this).InternalPage);

                ContentGrid.Children.Add(newPage.GetPage(this).InternalPage);
                ContentGrid.UpdateLayout();
            }
        }

        private void OnVisibilityChanged(object sender, VisibilityChangedEventArgs args)
        {
            CurrentPage?.GetPage(this).OnVisibilityChanged(args);
        }

        #region Dependency Properties

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content", typeof(object), typeof(HentaiFrame), new PropertyMetadata(default(object)));


        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public static readonly DependencyProperty ContentTransitionsProperty = DependencyProperty.Register(
            nameof(ContentTransitions), typeof(TransitionCollection), typeof(HentaiFrame),
            new PropertyMetadata(default(TransitionCollection)));


        public TransitionCollection ContentTransitions
        {
            get => (TransitionCollection) GetValue(ContentTransitionsProperty);
            set => SetValue(ContentTransitionsProperty, value);
        }

        #endregion

        #region Page Animations

        private async Task AnimateNavigatedFromIfCurrentPageNotNull(IPageAnimation pageAnimation,
            NavigationMode navigationMode,
            PageInsertionMode insertionMode, HentaiPageModel currentPage, HentaiPageModel newPage)
        {
            if (currentPage != null)
                if (insertionMode != PageInsertionMode.Sequential)
                    await AnimateNavigatingFromAsync(pageAnimation, navigationMode,
                        currentPage.GetPage(this).ActualAnimationContext,
                        insertionMode != PageInsertionMode.Sequential
                            ? newPage.GetPage(this).ActualAnimationContext
                            : null);
                else
                    await AnimateNavigatingFromAsync(pageAnimation, navigationMode,
                        currentPage.GetPage(this).ActualAnimationContext, null);
        }

        private async Task AnimateNavigatedToAndRemoveCurrentPageAsync(IPageAnimation pageAnimation,
            NavigationMode navigationMode, PageInsertionMode insertionMode, HentaiPageModel currentPage,
            HentaiPageModel newPage)
        {
            if (currentPage != null)
                if (insertionMode != PageInsertionMode.Sequential)
                {
                    await AnimateNavigatedToAsync(pageAnimation, navigationMode,
                        currentPage.GetPage(this).ActualAnimationContext,
                        newPage.GetPage(this).ActualAnimationContext);

                    ContentGrid.Children.Remove(currentPage.GetPage(this).InternalPage);
                }
                else
                {
                    await AnimateNavigatedToAsync(pageAnimation, navigationMode, null,
                        newPage.GetPage(this).ActualAnimationContext);
                }
        }

        private async Task AnimateNavigatingFromAsync(IPageAnimation pageAnimation, NavigationMode navigationMode,
            FrameworkElement previousPage, FrameworkElement nextPage)
        {
            if (IsFirstPage && ShowNavigationOnAppInAndOut && navigationMode == NavigationMode.Back)
                return;

            if (pageAnimation != null)
                if (navigationMode == NavigationMode.Back)
                    await pageAnimation.AnimateBackwardNavigatingFromAsync(previousPage, nextPage);
                else if (navigationMode != NavigationMode.Refresh)
                    await pageAnimation.AnimateForwardNavigatingFromAsync(previousPage, nextPage);
        }

        private async Task AnimateNavigatedToAsync(IPageAnimation pageAnimation, NavigationMode navigationMode,
            FrameworkElement previousPage, FrameworkElement nextPage)
        {
            if (IsFirstPage && ShowNavigationOnAppInAndOut &&
                (navigationMode == NavigationMode.New || navigationMode == NavigationMode.Forward))
            {
                if (nextPage != null)
                    nextPage.Opacity = 1;
                return;
            }

            if (pageAnimation != null)
                if (navigationMode == NavigationMode.Back)
                    await pageAnimation.AnimateBackwardNavigatedToAsync(previousPage, nextPage);
                else if (navigationMode != NavigationMode.Refresh)
                    await pageAnimation.AnimateForwardNavigatedToAsync(previousPage, nextPage);

            if (nextPage != null)
                nextPage.Opacity = 1;
        }

        #endregion

        #region Raise Events

        private void RaisePageOnNavigatedFrom(HentaiPageModel model, NavigationMode mode)
        {
            var page = model.GetPage(this);

            var args = new HentaiNavigationEventArgs
            {
                Content = page,
                SourcePageType = model.Type,
                Parameter = model.Parameter,
                NavigationMode = mode
            };

            page.OnNavigatedFromCore(args);
        }

        private async Task<bool> RaisePageOnNavigatingFromAsync(HentaiPageModel currentPage,
            HentaiPageModel nextPage, NavigationMode mode)
        {
            var page = currentPage.GetPage(this);

            var args = new HentaiNavigatingCancelEventArgs
            {
                Content = page,
                SourcePageType = currentPage.Type,
                NavigationMode = mode,
                Parameter = currentPage.Parameter
            };

            await page.OnNavigatingFromCoreAsync(args);

            if (!args.Cancel && nextPage != null)
            {
                var args2 = new HentaiNavigatingCancelEventArgs
                {
                    SourcePageType = nextPage.Type,
                    NavigationMode = mode,
                    Parameter = nextPage.Parameter
                };

                Navigating?.Invoke(this, args2);
                args.Cancel = args2.Cancel;
            }

            return args.Cancel;
        }

        private void RaisePageOnNavigatedTo(HentaiPageModel description, NavigationMode mode)
        {
            var page = description.GetPage(this);

            var args = new HentaiNavigationEventArgs
            {
                Content = page,
                SourcePageType = description.Type,
                Parameter = description.Parameter,
                NavigationMode = mode
            };
            page.OnNavigatedToCore(args);

            Navigated?.Invoke(this, args);

            OnNavigated(this, args);

            if (args.NavigationMode == NavigationMode.New)
                OnPageCreated(this, page);
        }

        #endregion
    }
}