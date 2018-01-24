using System.Threading.Tasks;
using Conet.Apis.Mastodon.Common;
using Conet.Apis.Mastodon.Model;
using Newtonsoft.Json.Linq;

namespace Conet.Apis.Mastodon.Api
{
    public partial class Search
    {
        /// <summary>
        /// Searching for content
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <param name="q">The search query</param>
        /// <param name="resolve">Whether to resolve non-local accounts</param>
        /// <returns>Returns <see cref="JToken"/>. If <see cref="q"/> is a URL, Mastodon will attempt to fetch the provided account or status. Otherwise, it will do a local account and hashtag search</returns>
        public static async Task<JToken> Searching(string domain, string q, bool resolve = false)
        {
            return await HttpHelper.GetAsync<JToken>($"{HttpHelper.HTTPS}{domain}{Constants.Search}", string.Empty, new []
            {
                ( nameof(q), q ),
                ( nameof(resolve), resolve.ToString() )
            });
        }
    }
}
