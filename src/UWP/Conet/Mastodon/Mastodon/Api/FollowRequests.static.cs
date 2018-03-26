using System.Net.Http;
using System.Threading.Tasks;
using Mastodon.Common;
using Mastodon.Model;

namespace Mastodon.Api
{
    public class FollowRequests
    {
        /// <summary>
        ///     Fetching a list of follow requests
        /// </summary>
        /// <param name="domain">Mastodon instance domain</param>
        /// <param name="token">AccessToken</param>
        /// <param name="max_id"></param>
        /// <param name="since_id"></param>
        /// <returns>Returns an array of <see cref="Account" /> which have requested to follow the authenticated user</returns>
        public static async Task<MastodonList<Account>> Fetching(string domain, string token, long max_id = 0,
            long since_id = 0, int limit = 40)
        {
            return await HttpHelper.GetListAsync<Account>(
                $"{HttpHelper.HTTPS}{domain}{Constants.FollowRequestsFetching}", token, max_id, since_id,
                (nameof(limit), limit.ToString()));
        }

        /// <summary>
        ///     Authorizing follow requests
        /// </summary>
        /// <param name="domain">Mastodon instance domain</param>
        /// <param name="token">AccessToken</param>
        /// <param name="id">The id of the account to authorize or reject</param>
        /// <returns></returns>
        public static async Task Authorize(string domain, string token, long id)
        {
            await HttpHelper.PostAsync<HttpContent>(
                $"{HttpHelper.HTTPS}{domain}{Constants.FollowRequestsAuthorize.Id(id.ToString())}", token, null);
        }

        /// <summary>
        ///     Rejecting follow requests
        /// </summary>
        /// <param name="domain">Mastodon instance domain</param>
        /// <param name="token">AccessToken</param>
        /// <param name="id">The id of the account to authorize or reject</param>
        /// <returns></returns>
        public static async Task Reject(string domain, string token, long id)
        {
            await HttpHelper.PostAsync<HttpContent>(
                $"{HttpHelper.HTTPS}{domain}{Constants.FollowRequestsReject.Id(id.ToString())}", token, null);
        }
    }
}