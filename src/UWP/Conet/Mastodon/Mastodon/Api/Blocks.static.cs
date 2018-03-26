using System.Net.Http;
using System.Threading.Tasks;
using Mastodon.Common;
using Mastodon.Model;

namespace Mastodon.Api
{
    public class Blocks
    {
        /// <summary>
        ///     Fetching a user's blocks
        /// </summary>
        /// <param name="domain">Mastodon instance domain</param>
        /// <param name="token">AccessToken</param>
        /// <param name="max_id"></param>
        /// <param name="since_id"></param>
        /// <returns>Returns an array of <see cref="Account" /> blocked by the authenticated user</returns>
        public static async Task<MastodonList<Account>> Fetching(string domain, string token, long max_id = 0,
            long since_id = 0, int limit = 40)
        {
            return await HttpHelper.GetListAsync<Account>($"{HttpHelper.HTTPS}{domain}{Constants.BlocksFetching}",
                token, max_id, since_id, (nameof(limit), limit.ToString()));
        }

        public static async Task<MastodonList<string>> Domain(string domain, string token, long max_id = 0,
            long since_id = 0, int limit = 40)
        {
            return await HttpHelper.GetListAsync<string>($"{HttpHelper.HTTPS}{domain}{Constants.BlocksDomain}",
                token, max_id, since_id, (nameof(limit), limit.ToString()));
        }

        public static async Task BlockDomain(string domain, string token, string domainToBlock)
        {
            await HttpHelper.PostAsync($"{HttpHelper.HTTPS}{domain}{Constants.BlocksDomain}", token,
                ("domain", domainToBlock));
        }

        public static async Task UnblockDomain(string domain, string token, string domainToUnblock)
        {
            await HttpHelper.DeleteAsync($"{HttpHelper.HTTPS}{domain}{Constants.BlocksDomain}", token,
                ("domain", new StringContent(domainToUnblock)));
        }
    }
}