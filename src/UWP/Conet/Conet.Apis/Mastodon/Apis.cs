using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Conet.Apis.Core;
using iHentai.Services;
using Newtonsoft.Json.Linq;

namespace Conet.Apis.Mastodon
{
    [ApiKey(nameof(Mastodon))]
    public class Apis : IConetApi
    {
        public ILoginData LoginDataGenerator => new LoginData();

        public Task<IInstanceData> Login(ILoginData data, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Type InstanceDataType { get; }

        public Task<(long Cursor, IEnumerable<JToken> Data)> HomeTimeline(IInstanceData data, int count = 20,
            long max_id = 0L,
            long since_id = 0L)
        {
            throw new NotImplementedException();
        }

        public Task<(string Uri, string CallbackUri)> GetOAuth(ILoginData data)
        {
            throw new NotImplementedException();
        }

        public Task<bool> OAuthResponseHandler(string response)
        {
            throw new NotImplementedException();
        }
    }
}