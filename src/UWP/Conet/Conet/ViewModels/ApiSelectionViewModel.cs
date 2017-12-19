using System;
using System.Collections.Generic;
using System.Linq;
using Conet.Apis.Core;
using Conet.Apis.Core.Models.Interfaces;
using iHentai.Mvvm;
using iHentai.Services;
using PropertyChanged;

namespace Conet.ViewModels
{
    public class ApiSelectionViewModel : ViewModel
    {
        public ApiSelectionViewModel()
        {
            SelectedService = Source.FirstOrDefault();
        }

        public List<ServiceSelectionBannerModel<ServiceTypes>> Source { get; } = Enum.GetNames(typeof(ServiceTypes))
            .Select(item => new ServiceSelectionBannerModel<ServiceTypes>(item)).ToList();

        public ServiceSelectionBannerModel<ServiceTypes> SelectedService { get; set; }

        [DependsOn(nameof(SelectedService))]
        public IConetApi Api =>
            SelectedService?.ServiceType.Get<IConetApi>();

        public ILoginData LoginData { get; private set; }

        public void OnSelectedServiceChanged()
        {
            LoginData = SelectedService?.ServiceType.Get<IConetApi>()?.LoginDataGenerator;
        }
    }
}