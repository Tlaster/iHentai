using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http.Configuration;
using iHentai.Basic.Helpers;

namespace iHentai.Services
{
    public class ApiHttpClientFactory : DefaultHttpClientFactory
    {
        public override HttpMessageHandler CreateMessageHandler()
        {
            return new ApiHttpClient();
        }
    }

    public class ApiHttpClient : HttpClientHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            CookieContainer.GetCookies(request.RequestUri)
                .Cast<Cookie>()
                .ToList()
                .ForEach(c => c.Expired = true);
            Singleton<ApiContainer>.Instance.HandleHttpMessage(ref request);
            return base.SendAsync(request, cancellationToken);
        }
    }
}