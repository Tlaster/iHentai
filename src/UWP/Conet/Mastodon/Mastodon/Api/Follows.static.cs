using System.Threading.Tasks;
using Mastodon.Common;
using Mastodon.Model;

namespace Mastodon.Api
{
    public class Follows
    {
        /// <summary>
        ///     Following a remote user
        /// </summary>
        /// <param name="domain">Mastodon instance domain</param>
        /// <param name="token">AccessToken</param>
        /// <param name="uri">username@domain of the person you want to follow</param>
        /// <returns>Returns the local representation of the followed account, as an <see cref="Account" /></returns>
        public static async Task<Account> Following(string domain, string token, string uri)
        {
            return await HttpHelper.PostAsync<Account, string>(
                $"{HttpHelper.HTTPS}{domain}{Constants.FollowsFollowing}", token, (nameof(uri), uri));
        }
    }
}