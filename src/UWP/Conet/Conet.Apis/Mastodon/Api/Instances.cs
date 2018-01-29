using System.Threading.Tasks;
using Conet.Apis.Mastodon.Common;
using Newtonsoft.Json.Linq;

namespace Conet.Apis.Mastodon.Api
{
    public static class Instances
    {
        public static async Task<JToken> Instance(string domain)
        {
            return await HttpHelper.GetAsync<JToken>($"{HttpHelper.HTTPS}{domain}{Constants.Instance}", string.Empty, null);
        }
    }
}
