using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace iHentai.Core.Common
{
    public class IncrementalLoadingCollection<TSource, IType> : ObservableCollection<IType>,
        ISupportIncrementalLoading
        where TSource : class, IIncrementalSource<IType>
    {
        private CancellationToken _cancellationToken;
        private bool _hasMoreItems;

        private bool _isLoading;
        private bool _refreshOnLoad;

        public IncrementalLoadingCollection()
            : this(Activator.CreateInstance<TSource>())
        {
        }

        public IncrementalLoadingCollection(TSource source)
        {
            DataSource = source ?? throw new ArgumentNullException(nameof(source));
            _hasMoreItems = true;
        }

        public TSource DataSource { get; }

        protected int CurrentPageIndex { get; set; }

        public bool IsLoading
        {
            get => _isLoading;

            private set
            {
                if (value == _isLoading) return;
                _isLoading = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsLoading)));

                if (_isLoading)
                    OnStartLoading?.Invoke();
                else
                    OnEndLoading?.Invoke();
            }
        }

        public Action OnStartLoading { get; set; }

        public Action OnEndLoading { get; set; }

        public Action<Exception> OnError { get; set; }

        public Action OnRefresh { get; set; }

        public Action OnRefreshEnd { get; set; }

        public bool HasMoreItems
        {
            get => !_cancellationToken.IsCancellationRequested && _hasMoreItems;

            private set
            {
                if (value == _hasMoreItems) return;
                _hasMoreItems = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(HasMoreItems)));
            }
        }

        public Task LoadMoreItemsAsync()
        {
            return LoadMoreItemsAsync(new CancellationToken(false));
        }

        public async Task RefreshAsync()
        {
            if (IsLoading)
            {
                _refreshOnLoad = true;
            }
            else
            {
                OnRefresh?.Invoke();
                Clear();
                CurrentPageIndex = 0;
                HasMoreItems = true;
                await LoadMoreItemsAsync();
                OnRefreshEnd?.Invoke();
            }
        }

        protected virtual async Task<IEnumerable<IType>> LoadDataAsync(CancellationToken cancellationToken)
        {
            var result = await DataSource.GetPagedItemsAsync(CurrentPageIndex++, cancellationToken);
            return result;
        }

        private async Task LoadMoreItemsAsync(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;

            try
            {
                if (!_cancellationToken.IsCancellationRequested)
                    try
                    {
                        IsLoading = true;
                        var data = await LoadDataAsync(_cancellationToken);
                        if (data != null && data.Any() && !_cancellationToken.IsCancellationRequested)
                            foreach (var item in data)
                                Add(item);
                        else
                            HasMoreItems = false;
                    }
                    catch (OperationCanceledException)
                    {
                        // The operation has been canceled using the Cancellation Token.
                        CurrentPageIndex--;
                    }
                    catch (Exception ex) // when (OnError != null)
                    {
                        CurrentPageIndex--;
                        OnError?.Invoke(ex);
                    }
            }
            finally
            {
                IsLoading = false;

                if (_refreshOnLoad)
                {
                    _refreshOnLoad = false;
                    await RefreshAsync();
                }
            }
        }
    }
}