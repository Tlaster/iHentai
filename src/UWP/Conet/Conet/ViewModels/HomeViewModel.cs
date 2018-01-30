using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conet.Apis.Core;
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
            Source = serviceType.Get<IConetApi>().GetHomeContent(_data.Get<IInstanceData>()).ToList();
        }

        public List<IConetViewModel> Source { get; }

        public string ServiceType { get; }

        protected override void OnStart()
        {
            base.OnStart();
            Frame.ClearBackStack();
        }
    }
}
