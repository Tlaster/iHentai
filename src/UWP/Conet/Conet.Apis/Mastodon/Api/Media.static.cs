using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Conet.Apis.Mastodon.Common;
using Conet.Apis.Mastodon.Model;
using Newtonsoft.Json.Linq;

namespace Conet.Apis.Mastodon.Api
{
    public partial class Media
    {
        /// <summary>
        /// Uploading a media attachment
        /// </summary>
        /// <param name="domain">Mastodon instance domain</param>
        /// <param name="token">AccessToken</param>
        /// <param name="file">Media to be uploaded</param>
        /// <returns>Returns an <see cref="AttachmentModel"/> that can be used when creating a status</returns>
        public static async Task<JToken> Uploading(string domain, string token, byte[] file)
        {
            return await HttpHelper.PostAsync<JToken, StreamContent>($"{HttpHelper.HTTPS}{domain}{Constants.MediaUploading}", token, new []
            {
                (nameof(file), new StreamContent(new MemoryStream(file)))
            });
        }
    }
}
