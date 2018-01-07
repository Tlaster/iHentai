using System.Collections.Generic;
using System.Threading.Tasks;
using Conet.Apis.Core.Models.Interfaces;
using iHentai.Services;

namespace Conet.Apis.Core
{
    public interface IConetApi : IApi, ICanLogin
    {
        Task<(long Cursor, IEnumerable<IStatusModel> Data)> HomeTimeline(IInstanceData data, int count = 20, long max_id = 0L, long since_id = 0L);
    }
}