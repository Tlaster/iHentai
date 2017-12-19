using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Conet.Apis.Core;
using Conet.Apis.Core.Models.Interfaces;
using iHentai.Services;

namespace Conet.Apis.Weibo
{
    [ApiKey(nameof(ServiceTypes.Weibo))]
    public class Apis : IConetApi
    {
        public Task<IEnumerable<IStatusModel>> HomeTimeline(long max_id = 0, long since_id = 0)
        {
            throw new NotImplementedException();
        }
    }
}
