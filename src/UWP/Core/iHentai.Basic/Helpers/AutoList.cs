using System;
using System.Linq;
using Microsoft.Toolkit.Uwp;

namespace iHentai.Basic.Helpers
{
    public class AutoList<TSource, T> : IncrementalLoadingCollection<TSource, T>
        where TSource : Microsoft.Toolkit.Collections.IIncrementalSource<T>
    {
        public AutoList()
        {
            OnError = OnErrorHandler;
            OnStartLoading = OnStartLoadingHandler;
            OnEndLoading = OnEndLoadingHandler;
        }

        public AutoList(TSource source) : base(source)
        {
            OnError = OnErrorHandler;
            OnStartLoading = OnStartLoadingHandler;
            OnEndLoading = OnEndLoadingHandler;
        }
        public TSource DataSource => Source;
        public bool IsError { get; private set; }
        public Exception ErrorException { get; private set; }
        public bool IsEmpty { get; private set; }

        private void OnEndLoadingHandler()
        {
            if (!IsError)
                IsEmpty = !this.Any();
        }

        private void OnStartLoadingHandler()
        {
            IsEmpty = false;
            IsError = false;
            ErrorException = null;
        }

        private void OnErrorHandler(Exception exception)
        {
            IsError = true;
            ErrorException = exception;
        }
    }
}