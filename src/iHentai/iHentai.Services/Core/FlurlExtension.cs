using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Html2Model;

namespace iHentai.Services.Core
{
    internal static class FlurlExtension
    {
        public static async Task<T> ReceiveHtml<T>(this Task<HttpResponseMessage> response) where T : class, new()
        {
            return HtmlConvert.DeserializeObject<T>(await response.ReceiveString());
        }

        public static async Task<object> ReceiveHtml(this Task<HttpResponseMessage> response, Type type)
        {
            return HtmlConvert.DeserializeObject(await response.ReceiveString(), type);
        }

        public static Task<T> GetHtmlAsync<T>(this IFlurlRequest request, CancellationToken cancellationToken = default,
            HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead) where T : class, new()
        {
            return request
                .SendAsync(HttpMethod.Get, cancellationToken: cancellationToken, completionOption: completionOption)
                .ReceiveHtml<T>();
        }

        public static Task<T> GetHtmlAsync<T>(this Url url, CancellationToken cancellationToken = default,
            HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead) where T : class, new()
        {
            return new FlurlRequest(url).GetHtmlAsync<T>(cancellationToken, completionOption);
        }
    }
}