using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using iHentai.Apis.Core;
using iHentai.Apis.Core.Models.Interfaces;
using iHentai.Basic;
using iHentai.Basic.Extensions;
using iHentai.Basic.Helpers;
using iHentai.Mvvm;
using iHentai.Services;
using Microsoft.Toolkit.Collections;

namespace iHentai.Core.ViewModels
{
    [Startup]
    public class GalleryViewModel : ViewModel
    {
        private readonly string _serviceType;

        public GalleryViewModel() : this(nameof(Apis.NHentai))
        {
        }

        public GalleryViewModel(string serviceType) :
            this(serviceType, null) //DO NOT use optional parameter since we use reflection at ServiceSelectionViewModel
        {
        }

        public GalleryViewModel(string serviceType, SearchOptionBase option)
        {
            _serviceType = serviceType;
            Source = new AutoList<GalleryDataSource, IGalleryModel>(new GalleryDataSource(serviceType.Get<IHentaiApi>(), option));
            if (option != null && !option.Keyword.IsEmpty())
                SearchPlaceholder = option.Keyword;
        }

        public string Title => _serviceType;

        public AutoList<GalleryDataSource, IGalleryModel> Source { get; }

        public string SearchPlaceholder { get; private set; } = Package.Current.DisplayName;

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

            Source.Refresh();
        }

        protected override void OnStart()
        {
            base.OnStart();
            Frame.ClearBackStack();
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
            return (await Apis.Gallery(Singleton<ApiContainer>.Instance.CurrentInstanceData, pageIndex, SearchOption, cancellationToken)).Gallery;
        }
    }
}