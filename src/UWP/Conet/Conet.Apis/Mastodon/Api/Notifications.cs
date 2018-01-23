using System.Threading.Tasks;
using Conet.Apis.Mastodon.Model;

namespace Conet.Apis.Mastodon.Api
{
    public partial class Notifications : Base
    {
        public Notifications(string domain, string accessToken) : base(domain, accessToken)
        {
        }

        /// <summary>
        /// Fetching a user's notifications
        /// </summary>
        /// <param name="max_id"></param>
        /// <param name="since_id"></param>
        /// <returns>Returns a list of <see cref="NotificationModel"/> for the authenticated user</returns>
        public async Task<ArrayModel<NotificationModel>> Fetching(int max_id = 0, int since_id = 0)
        {
            return await Fetching(Domain, AccessToken, max_id, since_id);
        }

        /// <summary>
        /// Getting a single notification
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns the <see cref="NotificationModel"/>.</returns>
        public async Task<NotificationModel> GetSingle(int id)
        {
            return await GetSingle(Domain, AccessToken, id);
        }

        /// <summary>
        /// Clearing notifications
        /// </summary>
        /// <returns>Deletes all notifications from the Mastodon server for the authenticated user. Returns an empty object.</returns>
        public async Task Clear()
        {
            await Clear(Domain, AccessToken);
        }
    }
}
