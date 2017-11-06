using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
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


        public IncrementalLoadingCollection<MainDataSource, IGalleryModel> Source { get; set; }

        public override async void Init()
        {
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
            Source = new IncrementalLoadingCollection<MainDataSource, IGalleryModel>(new MainDataSource(apis));
        }
    }

    public class MyClass : IIncrementalSource<string>
    {
        public async Task<IEnumerable<string>> GetPagedItemsAsync(int pageIndex,
            CancellationToken cancellationToken = default)
        {
            await Task.Delay(2000);
            return Enumerable.Range(0, 50).Select(item => item.ToString());
        }
    }

    public class MainDataSource : IIncrementalSource<IGalleryModel>
    {
        public MainDataSource(IHentaiApis apis)
        {
            Apis = apis;
        }

        public IHentaiApis Apis { get; set; }

        public async Task<IEnumerable<IGalleryModel>> GetPagedItemsAsync(int pageIndex,
            CancellationToken cancellationToken = default)
        {
            return (await Apis.Gallery(pageIndex)).Gallery;
        }
    }
}