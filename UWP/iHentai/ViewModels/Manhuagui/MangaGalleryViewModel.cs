using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using iHentai.Annotations;
using iHentai.Common.Collection;
using iHentai.Services.Core;
using iHentai.Services.Manhuagui;
using iHentai.Services.Manhuagui.Model;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Helpers;

namespace iHentai.ViewModels.Manhuagui
{
    class MangaGalleryViewModel : TabViewModelBase, IIncrementalSource<IMangaGallery>
    {
        private IMangaApi _api;
        private Func<int, Task<IEnumerable<IMangaGallery>>> _loadTask;
        public LoadingCollection<IIncrementalSource<IMangaGallery>, IMangaGallery> Source { get; }

        public MangaGalleryViewModel(IMangaApi api)
        {
            _api = api;
            ResetHome();
            Source = new LoadingCollection<IIncrementalSource<IMangaGallery>, IMangaGallery>(this);
        }

        public async Task<IEnumerable<IMangaGallery>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = new CancellationToken())
        {
            return await _loadTask.Invoke(pageIndex);
        }

        public void Search(string queryText)
        {
            _loadTask = page => _api.Search(queryText, page);
            Source?.Refresh();
        }

        public void ResetHome()
        {
            _loadTask = page =>_api.Home(page);
            Source?.Refresh();
        }
    }
}
