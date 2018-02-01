using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHentai.Basic.Controls;
using iHentai.Basic.Extensions;
using iHentai.Basic.Helpers;
using Newtonsoft.Json.Linq;

namespace Conet.Apis.Core.ViewModels
{
    public class HomeTimelineViewModel : ConetViewModelBase
    {
        public HomeTimelineViewModel(string serviceType, Guid messageGuid, Guid data) : base(messageGuid, data)
        {
            Source = new AutoList<TimelineDataSource, JToken>(new TimelineDataSource(_data, serviceType));
            ContentKey = $"{serviceType}Status";
        }

        public string ContentKey { get; }

        public override string Title => "Home".GetLocalized();
        public override Icons Icon { get; } = Icons.Home;
        public override int Badge { get; set; }

        public AutoList<TimelineDataSource, JToken> Source { get; }
    }
}
