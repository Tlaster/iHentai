using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using iHentai.Common.Collection;
using iHentai.Services.EHentai;
using iHentai.Services.EHentai.Model;
using Microsoft.Toolkit.Collections;

namespace iHentai.ViewModels.EHentai
{
    internal class GalleryViewModel : TabViewModelBase, IIncrementalSource<EHGallery>
    {
        private string _currentBaseUrl;
        private Func<int, Task<IEnumerable<EHGallery>>> _loadTask;

        public GalleryViewModel(EHApi api)
        {
            Api = api;
            Title = "EHentai";
            ResetHome();
            Source = new LoadingCollection<IIncrementalSource<EHGallery>, EHGallery>(this);
        }

        public GalleryViewModel(EHApi api, EHGalleryTag tag)
        {
            Api = api;
            Title = "EHentai";
            ResetTag(tag);
            Source = new LoadingCollection<IIncrementalSource<EHGallery>, EHGallery>(this);
        }

        public GalleryViewModel(EHApi api, string title, string link)
        {
            Api = api;
            Title = "EHentai";
            ResetLink(title, link);
            Source = new LoadingCollection<IIncrementalSource<EHGallery>, EHGallery>(this);
        }

        public SearchOption SearchOption { get; private set; } = new SearchOption();
        public bool AdvSearchEnabled { get; private set; } = true;
        public EHApi Api { get; }

        public LoadingCollection<IIncrementalSource<EHGallery>, EHGallery> Source { get; }

        public Task<IEnumerable<EHGallery>> GetPagedItemsAsync(int pageIndex, int pageSize,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return _loadTask.Invoke(pageIndex);
        }

        private void ResetLink(string title, string link)
        {
            AdvSearchEnabled = true;
            SearchOption = new SearchOption {Keyword = title};
            _currentBaseUrl = Api.Host;
            _loadTask = page => Api.Tag(link, page, Source.LastOrDefault()?.Id ?? 0);
            Source?.Refresh();
        }

        private void ResetTag(EHGalleryTag tag)
        {
            AdvSearchEnabled = true;
            SearchOption = new SearchOption {Keyword = tag.FullName};
            _currentBaseUrl = Api.Host;
            _loadTask = page => Api.Tag(tag.Link, page, Source.LastOrDefault()?.Id ?? 0);
            Source?.Refresh();
        }

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
            SearchOption = new FavSearchOption();
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