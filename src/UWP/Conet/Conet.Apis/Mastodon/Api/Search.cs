using System.Threading.Tasks;
using Conet.Apis.Mastodon.Model;

namespace Conet.Apis.Mastodon.Api
{
    public partial class Search : Base
    {
        public Search(string domain) : base(domain, string.Empty)
        {
        }

        /// <summary>
        /// Searching for content
        /// </summary>
        /// <param name="q">The search query</param>
        /// <param name="resolve">Whether to resolve non-local accounts</param>
        /// <returns>Returns <see cref="ResultsModel"/>. If <see cref="q"/> is a URL, Mastodon will attempt to fetch the provided account or status. Otherwise, it will do a local account and hashtag search</returns>
        public async Task<ResultsModel> Searching(string q, bool resolve = false)
        {
            return await Searching(Domain, q, resolve);
        }
    }
}
