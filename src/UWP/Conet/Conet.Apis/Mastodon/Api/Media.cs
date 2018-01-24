using System.Threading.Tasks;
using Conet.Apis.Mastodon.Model;
using Newtonsoft.Json.Linq;

namespace Conet.Apis.Mastodon.Api
{
    public partial class Media : Base
    {
        public Media(string domain, string accessToken) : base(domain, accessToken)
        {
        }

        /// <summary>
        /// Uploading a media attachment
        /// </summary>
        /// <param name="file">Media to be uploaded</param>
        /// <returns>Returns an <see cref="AttachmentModel"/> that can be used when creating a status</returns>
        public async Task<JToken> Uploading(byte[] file)
        {
            return await Uploading(Domain, AccessToken, file);
        }
    }
}
