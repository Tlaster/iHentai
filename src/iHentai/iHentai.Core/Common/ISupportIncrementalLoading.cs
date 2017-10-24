using System.Collections.Specialized;
using System.Threading.Tasks;

namespace iHentai.Core.Common
{
    public interface ISupportIncrementalLoading : INotifyCollectionChanged
    {

        Task LoadMoreItemsAsync();
        
        bool HasMoreItems { get; }
    }
}
