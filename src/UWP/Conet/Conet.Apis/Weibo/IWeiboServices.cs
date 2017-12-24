using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conet.Apis.Weibo.Models;
using Refit;

namespace Conet.Apis.Weibo
{
    internal interface IWeiboServices
    {
        [Get("statuses/home_timeline")]
        Task<StatusList> HomeTimeline(int count = 20, int page = 1, long max_id = 0, long since_id = 0,
            int base_app = 0, FeatureType feature = FeatureType.All, int trim_user = 0);
    }
}
