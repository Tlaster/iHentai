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
        private string _currentBaseUrl;
        public SearchOption SearchOption { get; private set; } = new SearchOption();
        public bool AdvSearchEnabled { get; private set; } = true;
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
            var queryParameter = SearchOption.ToSearchParameter();
            _loadTask = page => Api.Gallery(_currentBaseUrl + "?" + queryParameter, page);
            Source.Refresh();
        }

        public Task<IEnumerable<IGallery>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = new CancellationToken())
        {
            return _loadTask.Invoke(pageIndex);
        }

        public void ResetHome()
        {
            AdvSearchEnabled = true;
            SearchOption = new SearchOption();
            _currentBaseUrl = Api.Host;
            _loadTask = page => Api.Home(page);
            Source?.Refresh();
        }

        public void ResetWatched()
        {
            AdvSearchEnabled = true;
            SearchOption = new SearchOption();
            _currentBaseUrl = Api.Host + "watched";
            _loadTask = page => Api.Gallery(_currentBaseUrl, page);
            Source?.Refresh();
        }

        public void ResetFavorite()
        {
            AdvSearchEnabled = false;
            SearchOption = new SearchOption();
            _currentBaseUrl = Api.Host + "favorites.php";
            _loadTask = page => Api.Gallery(_currentBaseUrl, page);
            Source?.Refresh();
        }

        public void ResetPopular()
        {
            AdvSearchEnabled = true;
            SearchOption = new SearchOption();
            _currentBaseUrl = Api.Host;
            var url = Api.Host + "popular";
            _loadTask = page => Api.Gallery(url, page);
            Source?.Refresh();
        }
    }
}
