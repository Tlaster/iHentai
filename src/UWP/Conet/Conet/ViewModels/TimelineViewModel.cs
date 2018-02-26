using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conet.Apis.Core;
using iHentai.Basic.Helpers;
using iHentai.Mvvm;
using iHentai.Services;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp;
using Newtonsoft.Json.Linq;

namespace Conet.ViewModels
{
    //[Startup]
    public class TimelineViewModel : ViewModel
    {
        public TimelineViewModel(string serviceType)
        {
            ServiceType = serviceType;
            Source = new AutoList<TimelineDataSource, JToken>(new TimelineDataSource(ServiceType));
        }

        public AutoList<TimelineDataSource, JToken> Source { get; }

        public string ServiceType { get; }

        protected override void OnStart()
        {
            base.OnStart();
            Frame.ClearBackStack();
        }
    }

    public class TimelineDataSource : ConetDataSource<JToken>
    {
        private readonly string _serviceType;

        public TimelineDataSource(string serviceType)
        {
            _serviceType = serviceType;
        }

        protected override Task<(long Curser, IEnumerable<JToken> Data)> GetDataAsync(long curser, int pageSize,
            CancellationToken cancellationToken)
        {
            return _serviceType.Get<IConetApi>().HomeTimeline(Singleton<ApiContainer>.Instance.CurrentInstanceData, pageSize, curser);
        }
    }
}