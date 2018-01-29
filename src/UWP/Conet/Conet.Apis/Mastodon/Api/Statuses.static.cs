using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Conet.Apis.Mastodon.Common;
using Newtonsoft.Json.Linq;

namespace Conet.Apis.Mastodon.Api
{
    public partial class Statuses
    {
        /// <summary>
        /// Fetching a status
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="id"></param>
        /// <returns>Returns a <see cref="JToken"/></returns>
        public static async Task<JToken> Fetching(string domain, int id)
        {
            return await HttpHelper.GetAsync<JToken>($"{HttpHelper.HTTPS}{domain}{Constants.StatusesFetching.Id(id.ToString())}", string.Empty, null);
        }

        /// <summary>
        /// Getting status context
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="id"></param>
        /// <returns>Returns a <see cref="JToken"/></returns>
        public static async Task<JToken> Context(string domain, int id)
        {
            return await HttpHelper.GetAsync<JToken>($"{HttpHelper.HTTPS}{domain}{Constants.StatusesContext.Id(id.ToString())}", string.Empty, null);
        }

        /// <summary>
        /// Getting a card associated with a status
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="id"></param>
        /// <returns>Returns a <see cref="JToken"/></returns>
        public static async Task<JToken> Card(string domain, int id)
        {
            return await HttpHelper.GetAsync<JToken>($"{HttpHelper.HTTPS}{domain}{Constants.StatusesCard.Id(id.ToString())}", string.Empty, null);
        }

        /// <summary>
        /// Getting who reblogged a status
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="id"></param>
        /// <returns>Returns an array of <see cref="JToken"/></returns>
        public static async Task<JToken> RebloggedBy(string domain, int id)
        {
            return await HttpHelper.GetAsync<JToken>($"{HttpHelper.HTTPS}{domain}{Constants.StatusesRebloggedBy.Id(id.ToString())}", string.Empty, null);
        }

        /// <summary>
        /// Getting who favourited a status
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="id"></param>
        /// <returns>Returns an array of <see cref="JToken"/></returns>
        public static async Task<JToken> FavouritedBy(string domain, int id)
        {
            return await HttpHelper.GetAsync<JToken>($"{HttpHelper.HTTPS}{domain}{Constants.StatusesFavouritedBy.Id(id.ToString())}", string.Empty, null);
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
        /// <param name="visibility">(optional) either  <see cref="Constants.StatusVisibilityPublic"/>, <see cref="Constants.StatusVisibilityUnlisted"/>, <see cref="Constants.StatusVisibilityPrivate"/>, <see cref="Constants.StatusVisibilityDirect"/></param>
        /// <param name="media_ids">(optional) array of media IDs to attach to the status (maximum 4)</param>
        /// <returns></returns>
        public static async Task<JToken> Posting(string domain, string token, string status, int in_reply_to_id = 0, bool sensitive = false, string spoiler_text = "", string visibility = Constants.StatusVisibilityPublic, params int[] media_ids)
        {
            if (media_ids != null && media_ids.Length > 4)
            {
                throw new ArgumentOutOfRangeException(nameof(media_ids));
            }
            ICollection<(string, string)> param;
            if (media_ids != null && media_ids.Any())
            {
                param = HttpHelper.ArrayEncode(nameof(media_ids), media_ids.Select(v => v.ToString()).ToArray()).ToList();
            }
            else
            {
                param = new List<(string, string)>();
            }
            param.Add((nameof(status), status));
            param.Add((nameof(in_reply_to_id), in_reply_to_id.ToString()));
            param.Add((nameof(sensitive), sensitive.ToString()));
            param.Add((nameof(spoiler_text), spoiler_text));
            param.Add((nameof(visibility), visibility));
            return await HttpHelper.PostAsync<JToken, string>($"{HttpHelper.HTTPS}{domain}{Constants.StatusesPost}", token, param);
        }

        /// <summary>
        /// Deleting a status
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task Delete(string domain, string token, int id)
        {
            await HttpHelper.DeleteAsync($"{HttpHelper.HTTPS}{domain}{Constants.StatusesDelete.Id(id.ToString())}", token);
        }

        /// <summary>
        /// Reblogging a status
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns>Returns the target <see cref="JToken"/></returns>
        public static async Task<JToken> Reblog(string domain, string token, int id)
        {
            return await HttpHelper.PostAsync<JToken, HttpContent>($"{HttpHelper.HTTPS}{domain}{Constants.StatusesReblog.Id(id.ToString())}", token, null);
        }

        /// <summary>
        /// UnReblogging a status
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns>Returns the target <see cref="JToken"/></returns>
        public static async Task<JToken> UnReblog(string domain, string token, int id)
        {
            return await HttpHelper.PostAsync<JToken, HttpContent>($"{HttpHelper.HTTPS}{domain}{Constants.StatusesUnReblog.Id(id.ToString())}", token, null);
        }

        /// <summary>
        /// Favouriting a status
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns>Returns the target <see cref="JToken"/></returns>
        public static async Task<JToken> Favourite(string domain, string token, int id)
        {
            return await HttpHelper.PostAsync<JToken, HttpContent>($"{HttpHelper.HTTPS}{domain}{Constants.StatusesFavourite.Id(id.ToString())}", token, null);
        }

        /// <summary>
        /// UnFavouriting a status
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns>Returns the target <see cref="JToken"/></returns>
        public static async Task<JToken> UnFavourite(string domain, string token, int id)
        {
            return await HttpHelper.PostAsync<JToken, HttpContent>($"{HttpHelper.HTTPS}{domain}{Constants.StatusesUnFavourite.Id(id.ToString())}", token, null);
        }
    }
}
