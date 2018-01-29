using System.Threading.Tasks;
using Conet.Apis.Mastodon.Common;
using Conet.Apis.Mastodon.Models;
using Newtonsoft.Json.Linq;

namespace Conet.Apis.Mastodon.Api
{
    /// <summary>
    /// Retrieving a timeline
    /// </summary>
    partial class Timelines
    {
        public static async Task<ArrayModel<JToken>> Home(string domain, string token, long max_id = 0, long since_id = 0)
        {
            return await HttpHelper.GetArrayAsync<JToken>($"{HttpHelper.HTTPS}{domain}{Constants.TimelineHome}", token, max_id, since_id);
        }

        public static async Task<ArrayModel<JToken>> Public(string domain, long max_id = 0, long since_id = 0, bool local = false)
        {
            return await HttpHelper.GetArrayAsync<JToken>($"{HttpHelper.HTTPS}{domain}{Constants.TimelinePublic}", string.Empty, max_id, since_id, (nameof(local), local.ToString()));
        }

        public static async Task<ArrayModel<JToken>> HashTag(string domain, string hashtag, long max_id = 0, long since_id = 0, bool local = false)
        {
            return await HttpHelper.GetArrayAsync<JToken>($"{HttpHelper.HTTPS}{domain}{Constants.TimelineTag.Id(hashtag)}", string.Empty, max_id, since_id, (nameof(local), local.ToString()));
        }
    }
}
