using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Conet.Apis.Core;
using iHentai.Services;
using Newtonsoft.Json.Linq;

namespace Conet.Apis.Twitter
{
    [ApiKey(nameof(Twitter))]
    public class Apis : IConetApi
    {
        public ILoginData LoginDataGenerator => new LoginData();

        public Task<IInstanceData> Login(ILoginData data, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Type InstanceDataType { get; }

        public Task<(long Cursor, IEnumerable<JToken> Data)> HomeTimeline(IInstanceData data, int count = 20,
            long cursor = 0L)
        {
            throw new NotImplementedException();
        }

        public Task<(long Cursor, IEnumerable<JToken> Data)> UserTimeline(IInstanceData data, int count, long cursor = 0)
        {
            throw new NotImplementedException();
        }

        public Task<JToken> User(IInstanceData data, string uid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IConetViewModel> GetHomeContent(IInstanceData data)
        {
            throw new NotImplementedException();
        }
    }
}