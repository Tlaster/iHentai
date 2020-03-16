using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using iHentai.Common.Collection;
using iHentai.Services.Core;
using iHentai.Services.EHentai;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Helpers;

namespace iHentai.ViewModels.EHentai
{
    class GalleryViewModel : TabViewModelBase, IIncrementalSource<IGallery>
    {
        private Func<int, Task<IEnumerable<IGallery>>> _loadTask;
        public SearchOption SearchOption { get; } = new SearchOption();
        public EHApi Api { get; }

        public GalleryViewModel(EHApi api)
        {
            Api = api;
            Title = "EHentai";
            ResetHome();
            Source = new LoadingCollection<IIncrementalSource<IGallery>, IGallery>(this);
        }
        public LoadingCollection<IIncrementalSource<IGallery>, IGallery> Source { get; }

        public List<string> GetSearchSuggestion(string queryText)
        {
            return Api.GetSearchSuggestion(queryText);
        }

        public void Search(string queryText)
        {
            Api.SetSearchSuggestion(queryText);
            _loadTask = page => Api.Search(SearchOption, page);
            Source.Clear();
            Source.Refresh();
        }

        public Task<IEnumerable<IGallery>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = new CancellationToken())
        {
            return _loadTask.Invoke(pageIndex);
        }

        public void ResetHome()
        {
            _loadTask = page => Api.Home(page);
            if (Source != null)
            {
                Source.Clear();
                Source.Refresh();
            }
        }
    }
}
