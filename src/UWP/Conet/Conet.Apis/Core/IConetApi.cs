using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conet.Apis.Core.Models.Interfaces;
using iHentai.Services;

namespace Conet.Apis.Core
{
    public interface IConetApi : IApi
    {
        Task<IEnumerable<IStatusModel>> HomeTimeline(long max_id = 0L, long since_id = 0L);
    }
}
