using System.Net.Http;
using System.Threading.Tasks;
using Conet.Apis.Mastodon.Common;
using Conet.Apis.Mastodon.Model;

namespace Conet.Apis.Mastodon.Api
{
    public partial class FollowRequests
    {
        /// <summary>
        /// Fetching a list of follow requests
        /// </summary>
        /// <param name="domain">Mastodon instance domain</param>
        /// <param name="token">AccessToken</param>
        /// <param name="max_id"></param>
        /// <param name="since_id"></param>
        /// <returns>Returns an array of <see cref="AccountModel"/> which have requested to follow the authenticated user</returns>
        public static async Task<ArrayModel<AccountModel>> Fetching(string domain, string token, int max_id = 0, int since_id = 0)
        {
            return await HttpHelper.GetArrayAsync<AccountModel>($"{HttpHelper.HTTPS}{domain}{Constants.FollowRequestsFetching}", token, max_id, since_id);
        }

        /// <summary>
        /// Authorizing follow requests
        /// </summary>
        /// <param name="domain">Mastodon instance domain</param>
        /// <param name="token">AccessToken</param>
        /// <param name="id">The id of the account to authorize or reject</param>
        /// <returns></returns>
        public static async Task Authorize(string domain, string token, int id)
        {
            await HttpHelper.PostAsync<HttpContent>($"{HttpHelper.HTTPS}{domain}{Constants.FollowRequestsAuthorize.Id(id.ToString())}", token, null);
        }

        /// <summary>
        /// Rejecting follow requests
        /// </summary>
        /// <param name="domain">Mastodon instance domain</param>
        /// <param name="token">AccessToken</param>
        /// <param name="id">The id of the account to authorize or reject</param>
        /// <returns></returns>
        public static async Task Reject(string domain, string token, int id)
        {
            await HttpHelper.PostAsync<HttpContent>($"{HttpHelper.HTTPS}{domain}{Constants.FollowRequestsReject.Id(id.ToString())}", token, null);
        }



    }
}
