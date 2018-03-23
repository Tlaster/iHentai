using System;
using System.Diagnostics;
using System.Threading.Tasks;
using iHentai.Apis.Core;
using iHentai.Apis.Core.Models.Interfaces;
using iHentai.Basic.Helpers;
using iHentai.Mvvm;
using iHentai.Services;
using Nito.Mvvm;

namespace iHentai.Core.ViewModels
{
    public class GalleryDetailViewModel : ViewModel
    {
        private readonly string _serviceType;

        public GalleryDetailViewModel(string serviceType, IGalleryModel model)
        {
            Model = model;
            _serviceType = serviceType;
            DetailModel = NotifyTask.Create(GetDetailAsync);
        }

        public string Title => $"{Model.Title} - {_serviceType}";

        public NotifyTask<IGalleryDetailModel> DetailModel { get; }

        public IGalleryModel Model { get; }

        private async Task<IGalleryDetailModel> GetDetailAsync()
        {
            try
            {
                return await _serviceType.Get<IHentaiApi>().Detail(Singleton<ApiContainer>.Instance.CurrentInstanceData, Model);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
                throw;
            }
        }
    }
}