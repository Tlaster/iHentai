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
        public async Task<IEnumerable<IGallery>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = new CancellationToken())
        {
            return await Singleton<EHApi>.Instance.Home(pageIndex);
        }
    }

    class GalleryViewModel : TabViewModelBase
    {
        public GalleryViewModel()
        {
            Title = "EHentai";
        }
        public LoadingCollection<GallerySource, IGallery> Source { get; } = new LoadingCollection<GallerySource, IGallery>();
    }
}
