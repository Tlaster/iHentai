using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using iHentai.Core.Common;
using iHentai.Core.Common.Helpers;
using iHentai.Core.Views;
using iHentai.Mvvm;
using iHentai.Services.Core;
using iHentai.Services.Core.Models.Interfaces;

namespace iHentai.Core.ViewModels
{
    [Page(typeof(GalleryPage))]
    public class GalleryViewModel : ViewModel
    {
        private const string DefaultHentaiService = "default_hentai_service";

        public GalleryViewModel()
        {
        }

        public GalleryViewModel(ServiceTypes types, SearchOptionBase option)
        {
            ServiceType = types;
            Source = new IncrementalLoadingCollection<GalleryDataSource, IGalleryModel>(
                new GalleryDataSource(ServiceInstances.Instance[types], option));
        }

        public ServiceTypes ServiceType { get; set; }

        public IncrementalLoadingCollection<GalleryDataSource, IGalleryModel> Source { get; set; }

        protected override async void Init()
        {
            if (Source != null)
                return;
            IHentaiApis apis;
            if (Settings.Contains(DefaultHentaiService))
            {
                apis = ServiceInstances.Instance[Settings.Get(DefaultHentaiService, ServiceTypes.NHentai)];
            }
            else
            {
                var result = await Navigate<ServiceSelectionViewModel, (IHentaiApis apis, ServiceTypes types)>();
                apis = result.apis;
                Settings.Set(DefaultHentaiService, result.types);
            }
            Source = new IncrementalLoadingCollection<GalleryDataSource, IGalleryModel>(new GalleryDataSource(apis));
        }
    }

    public class GalleryDataSource : IIncrementalSource<IGalleryModel>
    {
        public GalleryDataSource(IHentaiApis apis, SearchOptionBase option = null)
        {
            Apis = apis;
            SearchOption = option;
        }

        public SearchOptionBase SearchOption { get; set; }
        public IHentaiApis Apis { get; set; }

        public async Task<IEnumerable<IGalleryModel>> GetPagedItemsAsync(int pageIndex,
            CancellationToken cancellationToken = default)
        {
            return (await Apis.Gallery(pageIndex, SearchOption)).Gallery;
        }
    }
}