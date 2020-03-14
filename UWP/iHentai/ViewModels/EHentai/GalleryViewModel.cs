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
    class GallerySource : IIncrementalSource<IGallery>
    {
        private readonly EHApi _api;
        public GallerySource(EHApi api)
        {
            _api = api;
        }
        public async Task<IEnumerable<IGallery>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = new CancellationToken())
        {
            return await _api.Home(pageIndex);
        }
    }

    class GalleryViewModel : TabViewModelBase
    {
        public EHApi Api { get; }

        public GalleryViewModel(EHApi api)
        {
            Api = api;
            Title = "EHentai";
            Source = new LoadingCollection<GallerySource, IGallery>(new GallerySource(Api));
        }
        public LoadingCollection<GallerySource, IGallery> Source { get; }
    }
}
