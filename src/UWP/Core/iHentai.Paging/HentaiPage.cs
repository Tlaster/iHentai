using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using iHentai.Paging.Animations;

namespace iHentai.Paging
{
    public class HentaiPage : ContentControl
    {
        public static readonly DependencyProperty TopAppBarProperty =
            DependencyProperty.Register(nameof(TopAppBar), typeof(AppBar), typeof(HentaiPage),
                new PropertyMetadata(default(AppBar), (o, args) => ((HentaiPage) o).OnUpdateTopAppBar()));

        public static readonly DependencyProperty BottomAppBarProperty =
            DependencyProperty.Register(nameof(BottomAppBar), typeof(AppBar), typeof(HentaiPage),
                new PropertyMetadata(default(AppBar), (o, args) => ((HentaiPage) o).OnUpdateBottomAppBar()));

        private Page _internalPage;
        private bool _isLoaded;

        public HentaiPage()
        {
            UseBackKeyToNavigate = true;
            IsSuspendable = true;
            UseAltLeftOrRightToNavigate = true;

            NavigationCacheMode = NavigationCacheMode.Required;

            HorizontalContentAlignment = HorizontalAlignment.Stretch;
            VerticalContentAlignment = VerticalAlignment.Stretch;
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
        }

        public IPageAnimation PageAnimation { get; set; }


        public HentaiFrame Frame { get; private set; }


        public FrameworkElement AnimationContext { get; set; }


        public FrameworkElement ActualAnimationContext
        {
            get
            {
                if (AnimationContext != null)
                    return AnimationContext;
                return this;
            }
        }


        public Page InternalPage => _internalPage ?? (_internalPage = new Page {Content = this});


        public NavigationCacheMode NavigationCacheMode { get; set; }


        public AppBar TopAppBar
        {
            get => (AppBar) GetValue(TopAppBarProperty);
            set => SetValue(TopAppBarProperty, value);
        }


        public AppBar BottomAppBar
        {
            get => (AppBar) GetValue(BottomAppBarProperty);
            set => SetValue(BottomAppBarProperty, value);
        }


        public bool IsSuspendable { get; protected set; }


        public bool UsePointerButtonsToNavigate { get; set; }


        public bool UseAltLeftOrRightToNavigate { get; set; }


        public bool UseBackKeyToNavigate { get; set; }


        public bool UseBackKeyToNavigateInWebView { get; set; }

        protected internal virtual void SetFrame(HentaiFrame frame, string pageKey)
        {
            Frame = frame;
        }

        protected internal virtual void OnKeyActivated(AcceleratorKeyEventArgs args)
        {
            // Must be empty
        }


        protected internal virtual void OnKeyUp(AcceleratorKeyEventArgs args)
        {
            // Must be empty
        }


        protected internal virtual void OnNavigatingFrom(HentaiNavigatingCancelEventArgs args)
        {
            // Must be empty
        }


        protected internal virtual Task OnNavigatingFromAsync(HentaiNavigatingCancelEventArgs args)
        {
            // Must be empty
            return null;
        }


        protected internal virtual void OnNavigatedFrom(HentaiNavigationEventArgs args)
        {
            // Must be empty
        }


        protected internal virtual void OnNavigatedTo(HentaiNavigationEventArgs args)
        {
            // Leave empty!
        }


        protected internal virtual void OnVisibilityChanged(VisibilityChangedEventArgs args)
        {
            // Must be empty
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }


        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _isLoaded = true;

            OnUpdateTopAppBar();
            OnUpdateBottomAppBar();
            OnDataContextChanged();
        }

        private void OnDataContextChanged()
        {
            if (_isLoaded)
                InternalPage.DataContext = DataContext;
        }

        private void OnUpdateTopAppBar()
        {
            if (_isLoaded && TopAppBar != null)
            {
                InternalPage.TopAppBar = TopAppBar;
                foreach (var item in Resources.Where(item => !(item.Value is DependencyObject)))
                    InternalPage.TopAppBar.Resources[item.Key] = item.Value;
            }
        }

        private void OnUpdateBottomAppBar()
        {
            if (_isLoaded && BottomAppBar != null)
            {
                InternalPage.BottomAppBar = BottomAppBar;
                foreach (var item in Resources.Where(item => !(item.Value is DependencyObject)))
                    InternalPage.BottomAppBar.Resources[item.Key] = item.Value;
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (InternalPage.TopAppBar != null)
                InternalPage.TopAppBar = null;

            if (InternalPage.BottomAppBar != null)
                InternalPage.BottomAppBar = null;
        }

        internal virtual async Task OnNavigatingFromCoreAsync(HentaiNavigatingCancelEventArgs e)
        {
            OnNavigatingFrom(e);
            var task = OnNavigatingFromAsync(e);
            if (task != null)
                await task;
        }

        internal void OnNavigatedFromCore(HentaiNavigationEventArgs e)
        {
            OnNavigatedFrom(e);
        }
        
        internal virtual void OnNavigatedToCore(HentaiNavigationEventArgs e)
        {
            OnNavigatedTo(e);
        }
    }
}