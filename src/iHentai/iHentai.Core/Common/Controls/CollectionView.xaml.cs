using System;
using System.Collections;
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
        
        private bool _isEmpty;
        private bool _isError;

        private bool _isRefreshing;

        public CollectionView()
        {
            InitializeComponent();
            CollectionListView.ItemAppearing += CollectionListView_ItemAppearing;
            CollectionListView.Refreshing += CollectionListView_Refreshing;
            CollectionListView.ItemTapped += CollectionListView_ItemTapped;
            var tapGestureRecognizer = new TapGestureRecognizer {NumberOfTapsRequired = 1};
            tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
            EmptyView.GestureRecognizers.Add(tapGestureRecognizer);
            ErrorView.GestureRecognizers.Add(tapGestureRecognizer);

        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Refresh();
        }

        private void CollectionListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return;
            }
            CollectionListView.SelectedItem = null;
        }

        public bool IsLoadingMore { get; private set; }

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
            private set
            {
                RefreshingView.IsVisible = value;
                CollectionListView.IsRefreshing = false;
                _isRefreshing = value;
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

        public bool IsError
        {
            get => _isError;
            set
            {
                ErrorView.IsVisible = value;
                _isError = value;
            }
        }


        private static void OnItemsSourceChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            (bindable as CollectionView).OnItemsSourceChanged(newvalue as ISupportIncrementalLoading);
        }

        private void OnItemsSourceChanged(ISupportIncrementalLoading value)
        {
            value.OnError = OnError;
            CollectionListView.ItemsSource = value as IEnumerable;
            Refresh();
        }

        private void OnError(Exception obj)
        {
            if (IsEmpty)
                return;
            IsError = true;
        }

        private void CollectionListView_Refreshing(object sender, EventArgs e)
        {
            Refresh();
        }

        private async void Refresh()
        {
            if (IsRefreshing || ItemsSource == null)
                return;
            IsError = false;
            IsEmpty = false;
            IsRefreshing = true;
            await ItemsSource?.RefreshAsync();
            IsRefreshing = false;
            if (!IsError) IsEmpty = (ItemsSource as ICollection).Count == 0;
        }

        private async void CollectionListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (IsLoadingMore || !ItemsSource.HasMoreItems || !ShouldLoadMore(e.Item)) return;
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