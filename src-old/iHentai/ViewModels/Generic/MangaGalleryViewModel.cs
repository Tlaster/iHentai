using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using iHentai.Common.Collection;
using iHentai.Services.Core;
using Microsoft.Toolkit.Collections;

namespace iHentai.ViewModels.Generic
{
    internal class MangaGalleryViewModel : TabViewModelBase, IIncrementalSource<IMangaGallery>
    {
        private Func<int, Task<IEnumerable<IMangaGallery>>> _loadTask;

        public MangaGalleryViewModel(IMangaApi api)
        {
            Api = api;
            Title = api.Name;
            ResetHome();
            Source = new LoadingCollection<IIncrementalSource<IMangaGallery>, IMangaGallery>(this);
        }

        public IMangaApi Api { get; }
        public LoadingCollection<IIncrementalSource<IMangaGallery>, IMangaGallery> Source { get; }

        public async Task<IEnumerable<IMangaGallery>> GetPagedItemsAsync(int pageIndex, int pageSize,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return await _loadTask.Invoke(pageIndex);
        }

        public void Search(string queryText)
        {
            _loadTask = page => Api.Search(queryText, page);
            Source?.Refresh();
        }

        public void ResetHome()
        {
            _loadTask = page => Api.Home(page);
            Source?.Refresh();
        }
    }
}