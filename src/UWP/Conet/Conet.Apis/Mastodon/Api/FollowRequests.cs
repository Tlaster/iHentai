using System.Threading.Tasks;
using Conet.Apis.Mastodon.Models;
using Newtonsoft.Json.Linq;

namespace Conet.Apis.Mastodon.Api
{
    public partial class FollowRequests : Base
    {
        /// <summary>
        /// Fetching a list of follow requests
        /// </summary>
        /// <param name="max_id"></param>
        /// <param name="since_id"></param>
        /// <returns>Returns an array of <see cref="AccountModel"/> which have requested to follow the authenticated user</returns>
        public async Task<ArrayModel<JToken>> Fetching(int max_id = 0, int since_id = 0)
        {
            return await Fetching(Domain, AccessToken, max_id, since_id);
        }

        /// <summary>
        /// Authorizing follow requests
        /// </summary>
        /// <param name="id">The id of the account to authorize or reject</param>
        /// <returns></returns>
        public async Task Authorize(int id)
        {
            await Authorize(Domain, AccessToken, id);
        }

        /// <summary>
        /// Rejecting follow requests
        /// </summary>
        /// <param name="id">The id of the account to authorize or reject</param>
        /// <returns></returns>
        public async Task Reject(int id)
        {
            await Reject(Domain, AccessToken, id);
        }

        public FollowRequests(string domain, string accessToken) : base(domain, accessToken)
        {
        }
    }
}
