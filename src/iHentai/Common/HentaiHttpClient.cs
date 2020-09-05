using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace iHentai.Common
{
    public interface ICustomHttpHandler
    {
        bool CanHandle(Uri uri);
        void Handle(HttpRequestMessage message);
    }

    public class HentaiHttpHandler : HttpClientHandler
    {
        private readonly List<ICustomHttpHandler> _handlers = new List<ICustomHttpHandler>();

        private HentaiHttpHandler()
        {
            if (SupportsAutomaticDecompression)
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            MaxConnectionsPerServer = 20;
        }

        public static HentaiHttpHandler Instance { get; } = new HentaiHttpHandler();

        public void RegisterHandler(ICustomHttpHandler handler)
        {
            _handlers.Add(handler);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
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
}