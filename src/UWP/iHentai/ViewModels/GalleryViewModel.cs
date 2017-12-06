using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using iHentai.Apis.Core;
using iHentai.Apis.Core.Models.Interfaces;
using iHentai.Helpers;
using iHentai.Mvvm;
using Microsoft.Toolkit.Uwp;

namespace iHentai.ViewModels
{
    public class GalleryViewModel : ViewModel
    {
        private readonly ServiceTypes _serviceType;

        public GalleryViewModel() : this(ServiceTypes.NHentai)
        {
            
        }

        public GalleryViewModel(ServiceTypes serviceType)
        {
            _serviceType = serviceType;
            Source = new IncrementalLoadingCollection<GalleryDataSource, IGalleryModel>(new GalleryDataSource(ServiceInstances.Instance[serviceType]));
        }
        public IncrementalLoadingCollection<GalleryDataSource, IGalleryModel> Source { get; } 
        
        public void GoDetail(IGalleryModel model)
        {
            Navigate<GalleryDetailViewModel>(_serviceType, model);
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


        public async Task<IEnumerable<IGalleryModel>> GetPagedItemsAsync(int pageIndex, int pageSize,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return (await Apis.Gallery(pageIndex, SearchOption)).Gallery;
        }
    }
}