using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using iHentai.Extensions;
using iHentai.Extensions.Models;
using iHentai.Services;
using iHentai.Services.Models.Core;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp;
using PropertyChanged;

namespace iHentai.ViewModels.Script
{
    internal class ScriptGalleryViewModel : ViewModelBase, IIncrementalSource<IGallery>
    {
        private Func<int, Task<IEnumerable<IGallery>>> _loadFunc;

        public ScriptGalleryViewModel(ScriptApi api)
        {
            Api = api;
            _loadFunc = page => Api.Home(page);
            Source = new IncrementalLoadingCollection<IIncrementalSource<IGallery>, IGallery>(this, onError: OnLoadError, onStartLoading: OnStartLoading, onEndLoading: OnEndLoading);
        }

        private void OnEndLoading()
        {
            IsLoading = false;
        }

        private void OnStartLoading()
        {
            Error = null;
            IsLoading = true;
        }

        private void OnLoadError(Exception obj)
        {
            Error = obj;
        }

        public ScriptApi Api { get; private set; }

        public Exception? Error { get; private set; }
        public bool IsLoading { get; private set; }
        [DependsOn(nameof(IsLoading))]
        public bool ShowLoadingState => Source.Count == 0;
        [DependsOn(nameof(Api))] public bool HasSearch => Api?.HasSearch() ?? false;

        public IncrementalLoadingCollection<IIncrementalSource<IGallery>, IGallery> Source { get; }

        public async Task<IEnumerable<IGallery>> GetPagedItemsAsync(int pageIndex, int pageSize,
            CancellationToken cancellationToken = default)
        {
            return await _loadFunc.Invoke(pageIndex);
        }

        public void Search(string queryText)
        {
            if (!Api.HasSearch())
            {
                return;
            }

            _loadFunc = page => Api.Search(queryText, page);
            Source.Clear();
            Source.RefreshAsync();
        }

        public void Reset()
        {
            _loadFunc = page => Api.Home(page);
            Source.Clear();
            Source.RefreshAsync();
        }
    }
}