using System.Threading.Tasks;
using Conet.Apis.Mastodon.Model;
using Newtonsoft.Json.Linq;

namespace Conet.Apis.Mastodon.Api
{
    public partial class Reports : Base
    {
        public Reports(string domain, string accessToken) : base(domain, accessToken)
        {
        }

        /// <summary>
        /// Fetching a user's reports
        /// </summary>
        /// <param name="max_id"></param>
        /// <param name="since_id"></param>
        /// <returns>Returns a list of <see cref="JToken"/> made by the authenticated user</returns>
        public async Task<ArrayModel<JToken>> Fetching(int max_id = 0, int since_id = 0)
        {
            return await Fetching(Domain, AccessToken, max_id, since_id);
        }

        /// <summary>
        /// Reporting a user
        /// </summary>
        /// <param name="account_id">The ID of the account to report</param>
        /// <param name="comment">A comment to associate with the report</param>
        /// <param name="status_ids">The IDs of statuses to report (can be an array)</param>
        /// <returns>Returns the finished <see cref="JToken"/>.</returns>
        public async Task<JToken> Reporting(int account_id, string comment, params int[] status_ids)
        {
            return await Reporting(Domain, AccessToken, account_id, comment, status_ids);
        }
    }
}
