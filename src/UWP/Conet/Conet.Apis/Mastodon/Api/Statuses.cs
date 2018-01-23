using System.Threading.Tasks;
using Conet.Apis.Mastodon.Model;

namespace Conet.Apis.Mastodon.Api
{
    public partial class Statuses : Base
    {
        public Statuses(string domain, string accessToken) : base(domain, accessToken)
        {
        }

        /// <summary>
        /// Fetching a status
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns>Returns a <see cref="StatusModel"/></returns>
        public async Task<StatusModel> Fetching(int id)
        {
            return await Fetching(Domain, id);
        }

        /// <summary>
        /// Getting status context
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns>Returns a <see cref="ContextModel"/></returns>
        public async Task<ContextModel> Context(int id)
        {
            return await Context(Domain, id);
        }

        /// <summary>
        /// Getting a card associated with a status
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns>Returns a <see cref="CardModel"/></returns>
        public async Task<CardModel> Card(int id)
        {
            return await Card(Domain, id);
        }

        /// <summary>
        /// Getting who reblogged a status
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns>Returns an array of <see cref="AccountModel"/></returns>
        public async Task<AccountModel> RebloggedBy(int id)
        {
            return await RebloggedBy(Domain, id);
        }

        /// <summary>
        /// Getting who favourited a status
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns>Returns an array of <see cref="AccountModel"/></returns>
        public async Task<AccountModel> FavouritedBy(int id)
        {
            return await FavouritedBy(Domain, id);
        }

        /// <summary>
        /// Posting a new status
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <param name="status">The text of the status</param>
        /// <param name="in_reply_to_id">(optional) local ID of the status you want to reply to</param>
        /// <param name="sensitive"> (optional) set this to mark the media of the status as NSFW</param>
        /// <param name="spoiler_text">(optional) text to be shown as a warning before the actual content</param>
        /// <param name="visibility">(optional) either  <see cref="StatusModel.STATUSVISIBILITY_PUBLIC"/>, <see cref="StatusModel.STATUSVISIBILITY_UNLISTED"/>, <see cref="StatusModel.STATUSVISIBILITY_PRIVATE"/>, <see cref="StatusModel.STATUSVISIBILITY_DIRECT"/></param>
        /// <param name="media_ids">(optional) array of media IDs to attach to the status (maximum 4)</param>
        /// <returns></returns>
        public async Task<StatusModel> Posting(string status, int in_reply_to_id = 0, bool sensitive = false, string spoiler_text = "", string visibility = StatusModel.STATUSVISIBILITY_PUBLIC, params int[] media_ids)
        {
            return await Posting(Domain, AccessToken, status, in_reply_to_id, sensitive, spoiler_text, visibility, media_ids);
        }

        /// <summary>
        /// Deleting a status
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete(int id)
        {
            await Delete(Domain, AccessToken, id);
        }

        /// <summary>
        /// Reblogging a status
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns>Returns the target <see cref="StatusModel"/></returns>
        public async Task<StatusModel> Reblog(int id)
        {
            return await Reblog(Domain, AccessToken, id);
        }

        /// <summary>
        /// UnReblogging a status
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns>Returns the target <see cref="StatusModel"/></returns>
        public async Task<StatusModel> UnReblog(int id)
        {
            return await UnReblog(Domain, AccessToken, id);
        }

        /// <summary>
        /// Favouriting a status
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns>Returns the target <see cref="StatusModel"/></returns>
        public async Task<StatusModel> Favourite(int id)
        {
            return await Favourite(Domain, AccessToken, id);
        }

        /// <summary>
        /// UnFavouriting a status
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns>Returns the target <see cref="StatusModel"/></returns>
        public async Task<StatusModel> UnFavourite(int id)
        {
            return await UnFavourite(Domain, AccessToken, id);
        }
    }
}
