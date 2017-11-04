using System;
using System.Collections;
using System.Net;
using System.Net.Http;
using System.Windows.Input;
using Flurl.Http;
using iHentai.Core.Common.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iHentai.Core.Common.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CollectionView : ContentView
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(ISupportIncrementalLoading), typeof(CollectionView),
                null, propertyChanged: OnItemsSourceChanged);

        public static readonly BindableProperty AutoRefreshProperty =
            BindableProperty.Create(nameof(AutoRefresh), typeof(bool), typeof(CollectionView), true);

        private bool _isEmpty;
        private bool _isError;
        private bool _isLoadingMore;
        private bool _isRefreshing;

        public ICommand ItemClickedCommand;

        public CollectionView()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.UWP)
                CollectionListView.LoadMoreRequest += CollectionListView_LoadMoreRequest;
            else
                CollectionListView.ItemAppearing += CollectionListView_ItemAppearing; //Not working properly on UWP
            CollectionListView.ScrollChanged += CollectionListViewOnScrollChanged;
            CollectionListView.Refreshing += CollectionListView_Refreshing;
            CollectionListView.ItemTapped += CollectionListView_ItemTapped;
            var tapGestureRecognizer = new TapGestureRecognizer {NumberOfTapsRequired = 1};
            tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
            EmptyView.GestureRecognizers.Add(tapGestureRecognizer);
            ErrorView.GestureRecognizers.Add(tapGestureRecognizer);
        }
        
        public bool IsLoadingMore
        {
            get => _isLoadingMore;
            private set
            {
                CollectionListView.IsPullToRefreshEnabled = !value;
                LoadMoreView.IsVisible = value;
                _isLoadingMore = value;
            }
        }

        public bool IsEmpty
        {
            get => _isEmpty;
            set
            {
                EmptyView.IsVisible = value;
                _isEmpty = value;
            }
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                //CollectionListView.IsPullToRefreshEnabled = !value;
                if (Device.RuntimePlatform == Device.UWP)
                    RefreshingView.IsVisible = value;
                else
                    CollectionListView.IsRefreshing = value;
                //if (value)
                //    Refresh();
            }
        }

        public DataTemplate ItemTemplate
        {
            get => CollectionListView.ItemTemplate;
            set => CollectionListView.ItemTemplate = value;
        }

        public object Header
        {
            get => CollectionListView.Header;
            set => CollectionListView.Header = value;
        }

        public DataTemplate HeaderTemplate
        {
            get => CollectionListView.HeaderTemplate;
            set => CollectionListView.HeaderTemplate = value;
        }

        public object Footer
        {
            get => CollectionListView.Footer;
            set => CollectionListView.Footer = value;
        }

        public DataTemplate FooterTemplate
        {
            get => CollectionListView.FooterTemplate;
            set => CollectionListView.FooterTemplate = value;
        }

        public ISupportIncrementalLoading ItemsSource
        {
            get => (ISupportIncrementalLoading) GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public bool AutoRefresh
        {
            get => (bool) GetValue(AutoRefreshProperty);
            set => SetValue(AutoRefreshProperty, value);
        }

        public bool IsError
        {
            get => _isError;
            set
            {
                ErrorView.IsVisible = value;
                _isError = value;
            }
        }

        public event EventHandler<ItemTappedEventArgs> ItemClicked;
        public event EventHandler<ScrollChangedEventArgs> ScrollChanged;

        private void CollectionListViewOnScrollChanged(object sender, ScrollChangedEventArgs scrollChangedEventArgs)
        {
            if(!IsRefreshing)
                ScrollChanged?.Invoke(this, scrollChangedEventArgs);
        }

        private async void CollectionListView_LoadMoreRequest(object sender, EventArgs e)
        {
            if (IsRefreshing || IsLoadingMore || !ItemsSource.HasMoreItems) return;
            IsLoadingMore = true;
            await ItemsSource.LoadMoreItemsAsync();
            IsLoadingMore = false;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Refresh();
        }

        private void CollectionListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            if (ItemClickedCommand != null && ItemClickedCommand.CanExecute(e.Item))
                ItemClickedCommand?.Execute(e.Item);
            ItemClicked?.Invoke(this, e);
            CollectionListView.SelectedItem = null;
        }


        private static void OnItemsSourceChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            (bindable as CollectionView).OnItemsSourceChanged(newvalue as ISupportIncrementalLoading);
        }

        private void OnItemsSourceChanged(ISupportIncrementalLoading value)
        {
            value.OnRefresh = OnRefresh;
            value.OnRefreshEnd = OnRefreshEnd;
            value.OnError = OnError;
            CollectionListView.ItemsSource = value;
            if (AutoRefresh)
                Refresh();
        }

        private void OnRefreshEnd()
        {
            IsRefreshing = false;
            if (!IsError) IsEmpty = ItemsSource.Count == 0;
        }

        private void OnRefresh()
        {
//            if (IsRefreshing || ItemsSource == null || IsLoadingMore)
//                return;
            IsError = false;
            IsEmpty = false;
            IsRefreshing = true;
        }

        private void OnError(Exception obj)
        {
            if (IsEmpty)
                return;
            switch (obj)
            {
                case FlurlHttpTimeoutException exception:
                    ErrorLabel.Text = "error_network_timeout".ToLocalized();
                    break;
                case FlurlHttpException exception:
                    ErrorLabel.Text = "error_network".ToLocalized();
                    break;
                case HttpRequestException exception:
                    ErrorLabel.Text = "error_network".ToLocalized();
                    break;
                case WebException exception:
                    ErrorLabel.Text = "error_network".ToLocalized();
                    break;
                default:
                    ErrorLabel.Text = "error".ToLocalized();
                    break;
            }
            IsError = true;
        }

        private void CollectionListView_Refreshing(object sender, EventArgs e)
        {
            if (ItemsSource != null)
            {
                Refresh();
            }
        }

        private async void Refresh()
        {
            await ItemsSource.RefreshAsync();
        }

        private async void CollectionListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (IsRefreshing || IsLoadingMore || !ItemsSource.HasMoreItems || !ShouldLoadMore(e.Item)) return;
            IsLoadingMore = true;
            await ItemsSource.LoadMoreItemsAsync();
            IsLoadingMore = false;
        }

        private bool ShouldLoadMore(object item)
        {
            if (!(ItemsSource is IList list)) return false;
            if (list.Count == 0)
                return true;
            var lastItem = list[list.Count - 1];
            return lastItem == item;
        }
    }
}