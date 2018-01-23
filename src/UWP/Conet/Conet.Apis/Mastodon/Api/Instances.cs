using System.Threading.Tasks;
using Conet.Apis.Mastodon.Common;
using Conet.Apis.Mastodon.Model;

namespace Conet.Apis.Mastodon.Api
{
    public static class Instances
    {
        public static async Task<InstanceModel> Instance(string domain)
        {
            return await HttpHelper.GetAsync<InstanceModel>($"{HttpHelper.HTTPS}{domain}{Constants.Instance}", string.Empty, null);
        }
    }
}
