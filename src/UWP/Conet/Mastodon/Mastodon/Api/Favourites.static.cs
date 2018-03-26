using System.Threading.Tasks;
using Mastodon.Common;
using Mastodon.Model;

namespace Mastodon.Api
{
    public class Favourites
    {
        /// <summary>
        ///     Fetching a user's favourites
        /// </summary>
        /// <param name="domain">Mastodon instance domain</param>
        /// <param name="token">AccessToken</param>
        /// <param name="max_id"></param>
        /// <param name="since_id"></param>
        /// <returns>Returns an array of <see cref="Status" /> favourited by the authenticated user</returns>
        public static async Task<MastodonList<Status>> Fetching(string domain, string token, long max_id = 0,
            long since_id = 0, int limit = 20)
        {
            return await HttpHelper.GetListAsync<Status>($"{HttpHelper.HTTPS}{domain}{Constants.FavouritesFetching}",
                token, max_id, since_id, (nameof(limit), limit.ToString()));
        }
    }
}