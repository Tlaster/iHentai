using System.Threading.Tasks;
using iHentai.Apis.Core;
using iHentai.Apis.Core.Models.Interfaces;
using iHentai.Mvvm;
using Nito.AsyncEx;
using Nito.Mvvm;

namespace iHentai.Core.ViewModels
{
    public class GalleryDetailViewModel : ViewModel
    {
        private readonly ServiceTypes _serviceType;

        public GalleryDetailViewModel(ServiceTypes serviceType, IGalleryModel model)
        {
            Model = model;
            _serviceType = serviceType;
            DetailModel = NotifyTask.Create(GetDetailAsync);
        }

        public NotifyTask<IGalleryDetailModel> DetailModel { get; }

        public IGalleryModel Model { get; }

        private Task<IGalleryDetailModel> GetDetailAsync()
        {
            return HentaiServices.Instance[_serviceType].Detail(Model);
        }
    }
}