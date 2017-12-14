using System;
using Microsoft.Toolkit.Uwp;

namespace iHentai.Basic.Helpers
{
    public class AutoList<TSource, T> : IncrementalLoadingCollection<TSource, T> where TSource : Microsoft.Toolkit.Collections.IIncrementalSource<T>
    {

        public TSource DataSource => Source;

        public AutoList(int itemsPerPage = 20, Action onStartLoading = null, Action onEndLoading = null, Action<Exception> onError = null) : base(itemsPerPage, onStartLoading, onEndLoading, onError)
        {
        }

        public AutoList(TSource source, int itemsPerPage = 20, Action onStartLoading = null, Action onEndLoading = null, Action<Exception> onError = null) : base(source, itemsPerPage, onStartLoading, onEndLoading, onError)
        {
        }
    }
}
