using System.Threading.Tasks;
using Conet.Apis.Mastodon.Model;

namespace Conet.Apis.Mastodon.Api
{
    public partial class Timelines : Base
    {
        public Timelines(string domain, string accessToken) : base(domain, accessToken)
        {
        }

        public async Task<ArrayModel<StatusModel>> Home(int max_id = 0, int since_id = 0)
        {
            return await Home(Domain, AccessToken, max_id, since_id);
        }

        public async Task<ArrayModel<StatusModel>> Public(int max_id = 0, int since_id = 0, bool local = false)
        {
            return await Public(Domain, max_id, since_id, local);
        }

        public async Task<ArrayModel<StatusModel>> HashTag(string hashtag, int max_id = 0, int since_id = 0, bool local = false)
        {
            return await HashTag(Domain, hashtag, max_id, since_id, local);
        }
    }
}
