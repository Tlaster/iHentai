using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace iHentai.Core.Common
{
    public interface ISupportIncrementalLoading : INotifyCollectionChanged
    {
        bool HasMoreItems { get; }

        Task LoadMoreItemsAsync();

        Action<Exception> OnError { get; set; }

        Task RefreshAsync();

        Action OnStartLoading { get; set; }

        Action OnEndLoading { get; set; }
    }
}