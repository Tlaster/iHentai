using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHentai.Apis.Core;
using iHentai.Mvvm;
using PropertyChanged;

namespace iHentai.ViewModels
{
    public class ServiceSelectionViewModel : ViewModel
    {
        public string[] Services { get; } = Enum.GetNames(typeof(ServiceTypes));

        public string SelectedService { get; set; }

        [DependsOn(nameof(SelectedService))]
        public ServiceTypes SelectedServiceType => Enum.Parse<ServiceTypes>(SelectedService);

        [DependsOn(nameof(SelectedService))]
        public IHentaiApis Api => ServiceInstances.Instance[SelectedServiceType];

        public string UserName { get; set; }

        public string Password { get; set; }

        public void Login()
        {
            
        }

        public ServiceSelectionViewModel()
        {
            SelectedService = Services[0];
        }
    }
}
