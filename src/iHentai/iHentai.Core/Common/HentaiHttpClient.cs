using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using iHentai.Services.Core;

namespace iHentai.Core.Common
{
    public class HentaiHttpClient : HttpClientHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var apis = ServiceInstances.Instance[request.RequestUri.Host];
            if (apis?.ImageRequestHeader != null)
                foreach (var item in apis.ImageRequestHeader)
                    request.Headers.Add(item.Key, item.Value);
            return base.SendAsync(request, cancellationToken);
        }
    }
}