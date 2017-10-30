using System;
using System.Collections;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace iHentai.Core.Common
{
    public interface ISupportIncrementalLoading : ICollection, INotifyCollectionChanged
    {
        bool HasMoreItems { get; }

        Action<Exception> OnError { get; set; }

        Action OnRefresh { get; set; }

        Action OnRefreshEnd { get; set; }

        Action OnStartLoading { get; set; }

        Action OnEndLoading { get; set; }

        Task LoadMoreItemsAsync();

        Task RefreshAsync();
    }
}