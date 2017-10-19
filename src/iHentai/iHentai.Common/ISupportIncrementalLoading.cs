using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace iHentai.Common
{
    public interface ISupportIncrementalLoading : INotifyCollectionChanged
    {

        Task LoadMoreItemsAsync();
        
        bool HasMoreItems { get; }
    }
}
