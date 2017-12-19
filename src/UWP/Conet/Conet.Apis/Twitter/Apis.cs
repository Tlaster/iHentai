using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conet.Apis.Core;
using Conet.Apis.Core.Models.Interfaces;
using iHentai.Services;

namespace Conet.Apis.Twitter
{
    [ApiKey(nameof(ServiceTypes.Twitter))]
    public class Apis : IConetApi
    {
        public ILoginData LoginDataGenerator => new LoginData();
        public Task<IEnumerable<IStatusModel>> HomeTimeline(long max_id = 0, long since_id = 0)
        {
            throw new NotImplementedException();
        }
    }
}
