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
        [Get("/statuses/friends_timeline")]
        Task<StatusList> HomeTimeline(int count = 20, long max_id = 0, long since_id = 0);

        [Get("/remind/unread_count")]
        Task<UnreadModel> Notification(bool unread_message = true, bool with_comment_attitude = true);
    }
}
