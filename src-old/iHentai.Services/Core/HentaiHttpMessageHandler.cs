using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http.Configuration;

namespace iHentai.Services.Core
{
    public class HentaiHttpClientFactory : DefaultHttpClientFactory
    {
        public override HttpMessageHandler CreateMessageHandler()
        {
            return HentaiHttpMessageHandler.Instance;
        }
    }

    public interface ICustomHttpHandler
    {
        bool CanHandle(Uri uri);
        void Handle(HttpRequestMessage message);
    }

    public class HentaiHttpMessageHandler : HttpClientHandler
    {
        public static HentaiHttpMessageHandler Instance { get; } = new HentaiHttpMessageHandler();
        private readonly List<ICustomHttpHandler> _handlers = new List<ICustomHttpHandler>();
        private HentaiHttpMessageHandler()
        {
            if (SupportsAutomaticDecompression)
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            MaxConnectionsPerServer = 20;
        }
        public void RegisterHandler(ICustomHttpHandler handler)
        {
            _handlers.Add(handler);
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            CookieContainer.GetCookies(request.RequestUri)
                .Cast<Cookie>()
                .ToList()
                .ForEach(c => c.Expired = true);
            _handlers.ForEach(it =>
            {
                if (it.CanHandle(request.RequestUri))
                {
                    it.Handle(request);
                }
            });
            return base.SendAsync(request, cancellationToken);
        }
    }

    class NoCookieHttpMessageHandler : HttpClientHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            CookieContainer.GetCookies(request.RequestUri)
                .Cast<Cookie>()
                .ToList()
                .ForEach(c => c.Expired = true);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
