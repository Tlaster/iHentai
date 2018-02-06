using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Markup;
using iHentai.Services;
using Microsoft.Toolkit.Collections;
using Newtonsoft.Json.Linq;

namespace Conet.Apis.Core
{
    public abstract class ConetDataSource<T> : IIncrementalSource<T>
    {
        protected long Curser = default;

        public async Task<IEnumerable<T>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = new CancellationToken())
        {
            if (pageIndex == 0)
            {
                Curser = default;
            }

            var data = await GetDataAsync(Curser, pageSize, cancellationToken);
            Curser = data.Curser;
            return data.Data;
        }

        protected abstract Task<(long Curser, IEnumerable<T> Data)> GetDataAsync(long curser, int pageSize, CancellationToken cancellationToken);
    }
    public class TimelineDataSource : ConetDataSource<JToken>
    {
        private readonly string _serviceType;
        private readonly Guid _data;

        public TimelineDataSource(Guid data, string serviceType)
        {
            _data = data;
            _serviceType = serviceType;
        }

        protected override Task<(long Curser, IEnumerable<JToken> Data)> GetDataAsync(long curser, int pageSize,
            CancellationToken cancellationToken)
        {
            return _serviceType.Get<IConetApi>().HomeTimeline(_data.Get<IInstanceData>(), pageSize, curser);
        }
    }
    public class UserTimelineDataSource : ConetDataSource<JToken>
    {
        private readonly string _serviceType;
        private readonly Guid _data;

        public UserTimelineDataSource(Guid data, string serviceType)
        {
            _data = data;
            _serviceType = serviceType;
        }

        protected override Task<(long Curser, IEnumerable<JToken> Data)> GetDataAsync(long curser, int pageSize,
            CancellationToken cancellationToken)
        {
            return _serviceType.Get<IConetApi>().UserTimeline(_data.Get<IInstanceData>(), pageSize, curser);
        }
    }
}
