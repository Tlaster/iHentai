using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using iHentai.Apis.Core;
using iHentai.Apis.Core.Models.Interfaces;
using iHentai.Basic.Extensions;
using iHentai.Basic.Helpers;
using iHentai.Mvvm;
using iHentai.Services;
using Microsoft.Toolkit.Collections;

namespace iHentai.Core.ViewModels
{
    public class GalleryViewModel : ViewModel
    {
        private ServiceTypes _serviceType;

        public string Title => _serviceType.ToString();
        
        public GalleryViewModel() : this(ServiceTypes.NHentai)
        {
        }

        public GalleryViewModel(ServiceTypes serviceType) : this(serviceType, null)
        {
        }

        public GalleryViewModel(ServiceTypes serviceType, SearchOptionBase option)
        {
            Init(serviceType, option);
        }

        public AutoList<GalleryDataSource, IGalleryModel> Source { get; private set; }

        public string SearchPlaceholder { get; private set; } = "iHentai";

        public void GoDetail(IGalleryModel model)
        {
            Navigate<GalleryDetailViewModel>(_serviceType, model).FireAndForget();
        }

        public void SearchSubmit()
        {
            if (Source.DataSource.SearchOption.Keyword.IsEmpty())
            {
                Source.DataSource.SearchOption = Source.DataSource.Apis.SearchOptionGenerator;
            }
            else
            {
                SearchPlaceholder = Source.DataSource.SearchOption.Keyword;
                Source.DataSource.SearchOption.SearchType = SearchTypes.Keyword;
            }
            Source.RefreshAsync().FireAndForget();
        }

        protected override void OnStart()
        {
            base.OnStart();
            Frame.ClearBackStack();
        }

        private void Init(ServiceTypes serviceType, SearchOptionBase option = null)
        {
            _serviceType = serviceType;
            Source = new AutoList<GalleryDataSource, IGalleryModel>(new GalleryDataSource(serviceType.Get<IHentaiApi>(),
                option));
            if (option != null && !option.Keyword.IsEmpty())
                SearchPlaceholder = option.Keyword;
        }
    }

    public class GalleryDataSource : IIncrementalSource<IGalleryModel>
    {
        public GalleryDataSource(IHentaiApi apis, SearchOptionBase option = null)
        {
            Apis = apis;
            SearchOption = option ?? apis.SearchOptionGenerator;
        }

        public SearchOptionBase SearchOption { get; set; }

        public IHentaiApi Apis { get; }

        public async Task<IEnumerable<IGalleryModel>> GetPagedItemsAsync(int pageIndex, int pageSize,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return (await Apis.Gallery(pageIndex, SearchOption, cancellationToken)).Gallery;
        }
    }
}