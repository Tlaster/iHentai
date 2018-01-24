using System.Threading.Tasks;
using Conet.Apis.Mastodon.Common;
using Conet.Apis.Mastodon.Model;
using Newtonsoft.Json.Linq;

namespace Conet.Apis.Mastodon.Api
{
    public partial class Follows
    {
        /// <summary>
        /// Following a remote user
        /// </summary>
        /// <param name="domain">Mastodon instance domain</param>
        /// <param name="token">AccessToken</param>
        /// <param name="uri">username@domain of the person you want to follow</param>
        /// <returns>Returns the local representation of the followed account, as an <see cref="AccountModel"/></returns>
        public static async Task<JToken> Following(string domain, string token, string uri)
        {
            return await HttpHelper.PostAsync<JToken, string>( $"{HttpHelper.HTTPS}{domain}{Constants.FollowsFollowing}", token, new[]
            {
                (nameof(uri), uri)
            });
        }
    }
}
