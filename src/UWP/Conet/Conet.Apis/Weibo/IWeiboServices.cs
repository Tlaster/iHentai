﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHentai.Services;
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
    }

    internal interface IWeiboServicesV2
    {
        [Get("/users/show.json")]
        Task<JObject> User(string access_token, string source, long uid, [Header(ApiHttpClient.FouceCookie)] bool fouceCache = false);
    }
}
