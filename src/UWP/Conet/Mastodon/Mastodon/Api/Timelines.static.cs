using System.Threading.Tasks;
using Mastodon.Common;
using Mastodon.Model;

namespace Mastodon.Api
{
    /// <summary>
    ///     Retrieving a timeline
    /// </summary>
    public static class Timelines
    {
        public static async Task<MastodonList<Status>> Home(string domain, string token, long max_id = 0,
            long since_id = 0, bool only_media = false, int limit = 20)
        {
            return await HttpHelper.GetListAsync<Status>($"{HttpHelper.HTTPS}{domain}{Constants.TimelineHome}", token,
                max_id, since_id,
                (nameof(only_media), only_media.ToString()),
                (nameof(limit), limit.ToString()));
        }

        public static async Task<MastodonList<Status>> Public(string domain, long max_id = 0, long since_id = 0,
            bool local = false, bool only_media = false, int limit = 20)
        {
            return await HttpHelper.GetListAsync<Status>($"{HttpHelper.HTTPS}{domain}{Constants.TimelinePublic}",
                string.Empty, max_id, since_id, (nameof(local), local.ToString()),
                (nameof(only_media), only_media.ToString()),
                (nameof(limit), limit.ToString()));
        }

        public static async Task<MastodonList<Status>> HashTag(string domain, string hashtag, long max_id = 0,
            long since_id = 0, bool local = false, bool only_media = false, int limit = 20)
        {
            return await HttpHelper.GetListAsync<Status>(
                $"{HttpHelper.HTTPS}{domain}{Constants.TimelineTag.Id(hashtag)}", string.Empty, max_id, since_id,
                (nameof(local), local.ToString()),
                (nameof(only_media), only_media.ToString()),
                (nameof(limit), limit.ToString()));
        }
        
        public static async Task<MastodonList<Status>> List(string domain, string token, string id, long max_id = 0,
            long since_id = 0, bool only_media = false, int limit = 20)
        {
            return await HttpHelper.GetListAsync<Status>($"{HttpHelper.HTTPS}{domain}{Constants.TimelineList.Id(id)}", token,
                max_id, since_id,
                (nameof(only_media), only_media.ToString()),
                (nameof(limit), limit.ToString()));
        }
    }
}