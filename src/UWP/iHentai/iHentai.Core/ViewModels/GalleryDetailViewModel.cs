using System.Threading.Tasks;
using iHentai.Apis.Core;
using iHentai.Apis.Core.Models.Interfaces;
using iHentai.Mvvm;
using iHentai.Services;
using Nito.Mvvm;

namespace iHentai.Core.ViewModels
{
    public class GalleryDetailViewModel : ViewModel
    {
        public string Title => $"{Model.Title} - {_serviceType}";

        private readonly string _serviceType;

        public GalleryDetailViewModel(string serviceType, IGalleryModel model)
        {
            Model = model;
            _serviceType = serviceType;
            DetailModel = NotifyTask.Create(GetDetailAsync);
        }

        public NotifyTask<IGalleryDetailModel> DetailModel { get; }

        public IGalleryModel Model { get; }

        private Task<IGalleryDetailModel> GetDetailAsync()
        {
            return _serviceType.Get<IHentaiApi>().Detail(Model);
        }
    }
}