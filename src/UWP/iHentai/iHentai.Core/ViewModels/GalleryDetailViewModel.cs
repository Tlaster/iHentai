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
        private readonly IInstanceData _data;
        private readonly string _serviceType;

        public GalleryDetailViewModel(string serviceType, IGalleryModel model, IInstanceData data)
        {
            Model = model;
            _data = data;
            _serviceType = serviceType;
            DetailModel = NotifyTask.Create(GetDetailAsync);
        }

        public string Title => $"{Model.Title} - {_serviceType}";

        public NotifyTask<IGalleryDetailModel> DetailModel { get; }

        public IGalleryModel Model { get; }

        private Task<IGalleryDetailModel> GetDetailAsync()
        {
            return _serviceType.Get<IHentaiApi>().Detail(_data, Model);
        }
    }
}