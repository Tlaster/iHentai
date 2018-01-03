using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Conet.Apis.Core;
using Conet.Apis.Core.Models.Interfaces;
using iHentai.Services;

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

        public Task<IEnumerable<IStatusModel>> HomeTimeline(long max_id = 0, long since_id = 0)
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