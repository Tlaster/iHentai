using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conet.Apis.Core;
using iHentai.Mvvm;
using iHentai.Services;
using PropertyChanged;

namespace Conet.ViewModels
{
    public class ApiSelectionViewModel : ViewModel
    {
        public List<ServiceSelectionBannerModel<ServiceTypes>> Source { get; } = Enum.GetNames(typeof(ServiceTypes))
            .Select(item => new ServiceSelectionBannerModel<ServiceTypes>(item)).ToList();

        public ServiceSelectionBannerModel<ServiceTypes> SelectedService { get; set; }
        
        [DependsOn(nameof(SelectedService))]
        public IConetApi Api =>
            SelectedService?.ServiceType.Get<IConetApi>();
    }
}
