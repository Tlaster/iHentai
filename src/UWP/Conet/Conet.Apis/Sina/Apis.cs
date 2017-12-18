using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conet.Apis.Core;
using Conet.Apis.Core.Models.Interfaces;

namespace Conet.Apis.Sina
{
    public class Apis : IConetApi
    {
        public Task<IEnumerable<IStatusModel>> GetStatus(long max_id = 0, long since_id = 0)
        {
            throw new NotImplementedException();
        }
    }
}
