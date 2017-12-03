using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using iHentai.Apis.Core;
using iHentai.Apis.Core.Models.Interfaces;
using iHentai.Helpers;
using iHentai.Mvvm;
using iHentai.Views;
using Microsoft.Toolkit.Uwp;
using GalleryPage = iHentai.Pages.GalleryPage;

namespace iHentai.ViewModels
{
    [Page(typeof(GalleryPage))]
    public class GalleryViewModel : ViewModel
    {
        protected override void Init()
        {
            base.Init();
            Source.RefreshAsync().FireAndForget();
        }

        public IncrementalLoadingCollection<GalleryDataSource, IGalleryModel> Source { get; set; } =
            new IncrementalLoadingCollection<GalleryDataSource, IGalleryModel>(
                new GalleryDataSource(ServiceInstances.Instance[ServiceTypes.NHentai]));

        public void GoDetail(IGalleryModel model)
        {
            Navigate<GalleryDetailViewModel>(model);
        }
    }


    public class GalleryDataSource : Microsoft.Toolkit.Collections.IIncrementalSource<IGalleryModel>
    {
        public GalleryDataSource(IHentaiApis apis, SearchOptionBase option = null)
        {
            Apis = apis;
            SearchOption = option;
        }

        public SearchOptionBase SearchOption { get; set; }
        public IHentaiApis Apis { get; set; }
        

        public async Task<IEnumerable<IGalleryModel>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = new CancellationToken())
        {
            return (await Apis.Gallery(pageIndex, SearchOption)).Gallery;
        }
    }
}
