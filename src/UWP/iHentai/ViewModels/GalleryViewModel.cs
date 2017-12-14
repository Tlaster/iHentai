using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using iHentai.Apis.Core;
using iHentai.Apis.Core.Models.Interfaces;
using iHentai.Extensions;
using iHentai.Helpers;
using iHentai.Mvvm;
using Microsoft.Toolkit.Collections;

namespace iHentai.ViewModels
{
    public class GalleryViewModel : ViewModel
    {
        private ServiceTypes _serviceType;

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
            Navigate<GalleryDetailViewModel>(_serviceType, model);
        }

        public void SearchSubmit()
        {
            if (Source.DataSource.SearchOption.Keyword.IsEmpty())
                return;

            SearchPlaceholder = Source.DataSource.SearchOption.Keyword;
            Source.DataSource.SearchOption.SearchType = SearchTypes.Keyword;
            Source.RefreshAsync().FireAndForget();
        }

        protected override void OnStart()
        {
            base.OnStart();
            Frame.ClearBackStack();
            //NavigationService.ClearBackStack();
        }

        //protected override async void OnStart()
        //{
        //    base.OnStart();
        //    if (Source == null)
        //        if ("default_hentai_service".HasValue() || await Navigate<ServiceSelectionViewModel, bool>())
        //            Init("default_hentai_service".Read(ServiceTypes.NHentai));
        //        else
        //            Close();
        //}

        private void Init(ServiceTypes serviceType, SearchOptionBase option = null)
        {
            _serviceType = serviceType;
            Source = new AutoList<GalleryDataSource, IGalleryModel>(new GalleryDataSource(HentaiServices.Instance[serviceType], option), onError: OnError);
            if (option != null && !option.Keyword.IsEmpty())
                SearchPlaceholder = option.Keyword;
        }

        private void OnError(Exception obj)
        {
            
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

        public IHentaiApi Apis { get; set; }

        public async Task<IEnumerable<IGalleryModel>> GetPagedItemsAsync(int pageIndex, int pageSize,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return (await Apis.Gallery(pageIndex, SearchOption)).Gallery;
        }
    }
}