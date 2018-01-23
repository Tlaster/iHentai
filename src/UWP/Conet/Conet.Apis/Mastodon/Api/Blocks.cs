using System.Threading.Tasks;
using Conet.Apis.Mastodon.Model;

namespace Conet.Apis.Mastodon.Api
{
    public partial class Blocks : Base
    {
        /// <summary>
        /// Fetching a user's blocks
        /// </summary>
        /// <param name="max_id"></param>
        /// <param name="since_id"></param>
        /// <returns>Returns an array of <see cref="AccountModel"/> blocked by the authenticated user</returns>
        public async Task<ArrayModel<AccountModel>> Fetching(int max_id = 0, int since_id = 0)
        {
            return await Fetching(Domain, AccessToken, max_id, since_id);
        }

        public Blocks(string domain, string accessToken) : base(domain, accessToken)
        {
        }
    }
}
