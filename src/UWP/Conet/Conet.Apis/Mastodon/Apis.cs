using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conet.Apis.Core;
using Conet.Apis.Core.Models.Interfaces;

namespace Conet.Apis.Mastodon
{
    public class Apis : IConetApi
    {
        public Task<IEnumerable<IStatusModel>> GetStatus(long max_id = 0L, long since_id = 0L)
        {
            throw new NotImplementedException();
        }
    }
}
