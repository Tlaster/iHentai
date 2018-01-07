using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conet.Apis.Core;
using Conet.Apis.Core.Models.Interfaces;
using iHentai.Basic.Helpers;
using iHentai.Mvvm;
using iHentai.Services;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp;

namespace Conet.ViewModels
{
    [Startup]
    public class TimelineViewModel : ViewModel
    {
        private readonly string _serviceType;
        private readonly Guid _data;

        public TimelineViewModel(string serviceType, Guid data)
        {
            _data = data;
            _serviceType = serviceType;
            Source = new AutoList<TimelineDataSource, IStatusModel>(new TimelineDataSource(_data, _serviceType));
        }

        public AutoList<TimelineDataSource, IStatusModel> Source { get; }
    }

    public class TimelineDataSource : ConetDataSource<IStatusModel>
    {
        private readonly string _serviceType;
        private readonly Guid _data;

        public TimelineDataSource(Guid data, string serviceType)
        {
            _data = data;
            _serviceType = serviceType;
        }

        protected override Task<(long Curser, IEnumerable<IStatusModel> Data)> GetDataAsync(long curser, int pageSize, CancellationToken cancellationToken)
        {
            return _serviceType.Get<IConetApi>().HomeTimeline(_data.Get<IInstanceData>(), pageSize, curser, 0);
        }
    }
}