using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Mastodon.Common;
using Mastodon.Model;

namespace Mastodon.Api
{
    public class Media
    {
        /// <summary>
        ///     Uploading a media attachment
        /// </summary>
        /// <param name="domain">Mastodon instance domain</param>
        /// <param name="token">AccessToken</param>
        /// <param name="file">Media to be uploaded</param>
        /// <returns>Returns an <see cref="Attachment" /> that can be used when creating a status</returns>
        public static async Task<Attachment> Uploading(string domain, string token, byte[] file,
            string description = null)
        {
            return await HttpHelper.PostAsync<Attachment, HttpContent>(
                $"{HttpHelper.HTTPS}{domain}{Constants.MediaUploading}", token,
                (nameof(file), new StreamContent(new MemoryStream(file))),
                (nameof(description), new StringContent(description)));
        }
    }
}