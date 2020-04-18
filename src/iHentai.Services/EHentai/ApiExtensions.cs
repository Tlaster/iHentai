using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using iHentai.Html;

namespace iHentai.Services.EHentai
{
    internal static class ApiExtensions
    {
        public static Task<T> GetHtmlAsync<T>(
            this Url url,
            CancellationToken cancellationToken = default,
            HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
        {
            return new FlurlRequest(url).GetHtmlAsync<T>(cancellationToken, completionOption);
        }

        public static Task<T> GetHtmlAsync<T>(
            this IFlurlRequest request,
            CancellationToken cancellationToken = default,
            HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
        {
            return request.SendAsync(HttpMethod.Get, null, cancellationToken, completionOption).ReceiveHtml<T>();
        }

        public static async Task<T> ReceiveHtml<T>(this Task<IFlurlResponse> response)
        {
            using var resp = await response;
            if (resp == null)
            {
                return default;
            }

            using var result = await resp.GetStreamAsync();
            return HtmlConvert.DeserializeObject<T>(result);
        }
    }
}