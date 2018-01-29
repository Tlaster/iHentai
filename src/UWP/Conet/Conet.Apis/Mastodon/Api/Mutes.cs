using System.Threading.Tasks;
using Conet.Apis.Mastodon.Models;
using Newtonsoft.Json.Linq;

namespace Conet.Apis.Mastodon.Api
{
    public partial class Mutes : Base
    {
        public Mutes(string domain, string accessToken) : base(domain, accessToken)
        {
        }
        /// <summary>
        /// Fetching a user's mutes
        /// </summary>
        /// <param name="max_id"></param>
        /// <param name="since_id"></param>
        /// <returns>Returns an array of <see cref="AccountModel"/> muted by the authenticated user</returns>
        public async Task<ArrayModel<JToken>> Fetching(int max_id = 0, int since_id = 0)
        {
            return await Fetching(Domain, AccessToken, max_id, since_id);
        }
    }
}
