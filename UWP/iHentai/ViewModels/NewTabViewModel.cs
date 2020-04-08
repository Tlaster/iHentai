using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHentai.Activities;
using iHentai.Activities.EHentai;
using iHentai.Activities.Manhuagui;
using iHentai.Services;
using Microsoft.Toolkit.Helpers;

namespace iHentai.ViewModels
{
    class NewTabViewModel : TabViewModelBase
    {
        public NewTabViewModel()
        {
            Title = "New Tab";
        }

        public List<IServiceModel> Services => Singleton<ServiceManager>.Instance.Services;
    }
}
