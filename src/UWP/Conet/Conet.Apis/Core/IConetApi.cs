using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conet.Apis.Core.Models.Interfaces;

namespace Conet.Apis.Core
{
    public interface IConetApi
    {
        Task<IEnumerable<IStatusModel>> GetStatus(long max_id = 0L, long since_id = 0L);
    }
}
