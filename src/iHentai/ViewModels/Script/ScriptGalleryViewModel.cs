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
            Source = new IncrementalLoadingCollection<IIncrementalSource<IGallery>, IGallery>(this);
            Reset();
        }

        public ScriptApi Api { get; private set; }

        public bool HasSearch { get; private set; }

        public IncrementalLoadingCollection<IIncrementalSource<IGallery>, IGallery> Source { get; }

        public async Task<IEnumerable<IGallery>> GetPagedItemsAsync(int pageIndex, int pageSize,
            CancellationToken cancellationToken = default)
        {
            return await _loadFunc.Invoke(pageIndex);
        }

        public void Search(string queryText)
        {
            _loadFunc = page => Api.Search(queryText, page);
            Source.Clear();
            Source.RefreshAsync();
        }

        public void Reset()
        {
            _loadFunc = page => Api.Home(page);
            Api.HasSearch().ContinueWith(it => HasSearch = it.Result);
            Source.Clear();
            Source.RefreshAsync();
        }
    }
}