using System.Threading.Tasks;
using Conet.Apis.Mastodon.Model;

namespace Conet.Apis.Mastodon.Api
{
    public partial class Follows : Base
    {

        /// <summary>
        /// Following a remote user
        /// </summary>
        /// <param name="uri">username@domain of the person you want to follow</param>
        /// <returns>Returns the local representation of the followed account, as an <see cref="AccountModel"/></returns>
        public async Task<AccountModel> Following(string uri)
        {
            return await Following(Domain, AccessToken, uri);
        }

        public Follows(string domain, string accessToken) : base(domain, accessToken)
        {
        }
    }
}
