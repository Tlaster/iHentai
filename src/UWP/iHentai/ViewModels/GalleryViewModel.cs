﻿using System.Collections.Generic;
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

        public GalleryViewModel(ServiceTypes serviceType, SearchOptionBase option = null)
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
            NavigationService.ClearBackStack();
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
            Source = new GalleryDataSource(ServiceInstances.Instance[serviceType], option)
                .ToList<GalleryDataSource, IGalleryModel>();
            if (option != null && !option.Keyword.IsEmpty())
                SearchPlaceholder = option.Keyword;
        }
    }

    public class GalleryDataSource : IIncrementalSource<IGalleryModel>
    {
        public GalleryDataSource(IHentaiApis apis, SearchOptionBase option = null)
        {
            Apis = apis;
            SearchOption = option ?? apis.SearchOptionGenerator;
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