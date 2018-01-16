using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Refit;

namespace Conet.Apis.Weibo
{
    internal interface IWeiboServices
    {
        [Get("/statuses/friends_timeline")]
        Task<JObject> HomeTimeline(string access_token, string source, int count = 20, long max_id = 0, long since_id = 0);

        [Get("/remind/unread_count")]
        Task<JObject> Notification(string access_token, string source, bool unread_message = true, bool with_comment_attitude = true);

        [Get("/users/show")]
        Task<JObject> User(string access_token, long uid);
    }
}
