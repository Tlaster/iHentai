using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHentai.Apis.Core;
using iHentai.Apis.Core.Models;
using iHentai.Mvvm;
using PropertyChanged;

namespace iHentai.ViewModels
{
    public class ServiceSelectionViewModel : ViewModel<bool>
    {
        public List<ServiceSelectionBannerModel> Source { get; } = Enum.GetNames(typeof(ServiceTypes))
            .Select(item => new ServiceSelectionBannerModel(item)).ToList();

        public ServiceSelectionBannerModel SelectedService { get; set; }
        
        [DependsOn(nameof(SelectedService))]
        public IHentaiApis Api => SelectedService == null ? null : ServiceInstances.Instance[SelectedService.ServiceType];

        public string UserName { get; set; }

        public string Password { get; set; }

        public void Login()
        {
            
        }
    }
}
