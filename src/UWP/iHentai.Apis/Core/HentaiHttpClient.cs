using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http.Configuration;

namespace iHentai.Apis.Core
{
    public class HentaiHttpClientFactory : DefaultHttpClientFactory
    {
        public override HttpMessageHandler CreateMessageHandler()
        {
            return new HentaiHttpClient(); 
        }
    }

    public class HentaiHttpClient : HttpClientHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var apis = HentaiServices.Instance[request.RequestUri.Host];
            if (apis?.RequestHeader != null)
            {
                //Clear all cookie to avoid some annoying issues
                CookieContainer.GetCookies(request.RequestUri)
                    .Cast<Cookie>()
                    .ToList()
                    .ForEach(c => c.Expired = true);

                request.Headers.Clear();
                foreach (var item in apis.RequestHeader)
                    request.Headers.Add(item.Key, item.Value);
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}