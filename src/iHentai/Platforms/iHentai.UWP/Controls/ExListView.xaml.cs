using System.Linq;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace iHentai.UWP.Controls
{
    [TemplatePart(Name = "FailedView", Type = typeof(Border))]
    [TemplatePart(Name = "EmptyView", Type = typeof(Border))]
    [TemplatePart(Name = "RefreshView", Type = typeof(Border))]
    public partial class ExListView : PullToRefreshListView
    {
        // Using a DependencyProperty as the backing store for IsLoading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(ExListView),
                new PropertyMetadata(false, OnIsLoadingChanged));

        // Using a DependencyProperty as the backing store for LoadError.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoadErrorProperty =
            DependencyProperty.Register(nameof(IsError), typeof(bool), typeof(ExListView),
                new PropertyMetadata(false, OnIsErrorChanged));

        private Border _emptyView;

        private Border _failedView;
        private Border _refreshView;


        public ExListView()
        {
            InitializeComponent();
            //PullToRefreshLabel = ResourceHelper.GetString("PullToRefresh");
            //ReleaseToRefreshLabel = ResourceHelper.GetString("ReleaseToRefresh");
            Items.VectorChanged += Items_VectorChanged;
        }

        public bool IsLoading
        {
            get => (bool) GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, value);
        }

        public bool IsError
        {
            get => (bool) GetValue(LoadErrorProperty);
            set => SetValue(LoadErrorProperty, value);
        }

        private static void OnIsLoadingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ExListView).OnIsLoadingChanged((bool) e.NewValue);
        }

        private void OnIsLoadingChanged(bool newValue)
        {
            CheckForEmptyView();
            if (_refreshView != null)
                _refreshView.Visibility = newValue && !Items.Any() ? Visibility.Visible : Visibility.Collapsed;
        }

        private static void OnIsErrorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ExListView).OnIsErrorChanged((bool) e.NewValue);
        }

        private void OnIsErrorChanged(bool newValue)
        {
            _failedView.Visibility = newValue && !Items.Any() ? Visibility.Visible : Visibility.Collapsed;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _emptyView = GetTemplateChild("EmptyView") as Border;
            _failedView = GetTemplateChild("FailedView") as Border;
            _refreshView = GetTemplateChild("RefreshView") as Border;
        }

        private void Items_VectorChanged(IObservableVector<object> sender, IVectorChangedEventArgs @event)
        {
            CheckForEmptyView();
        }

        private void CheckForEmptyView()
        {
            if (_emptyView != null)
                _emptyView.Visibility = Items.Any() || IsError || IsLoading ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}