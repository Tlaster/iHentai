using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHentai.Mvvm;
using iHentai.Services;

namespace Conet.ViewModels
{
    [Startup]
    public class HomeViewModel : ViewModel
    {
        private readonly Guid _data;

        public HomeViewModel(string serviceType, Guid data)
        {
            _data = data;
            ServiceType = serviceType;
        }
        
        public string ServiceType { get; }

        protected override void OnStart()
        {
            base.OnStart();
            Frame.ClearBackStack();
        }
    }
}
