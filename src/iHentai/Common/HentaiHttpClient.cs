using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using iHentai.Scripting.Runtime;
using Windows.Foundation;

namespace iHentai.Common
{
    public interface ICustomHttpHandler
    {
        Task<bool> CanHandle(Uri uri);
        Task Handle(HttpRequestMessage message);
    }

    public class HentaiHttpHandler : HttpClientHandler, IScriptHttpInterceptor
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

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            await Handle(request);
            return await base.SendAsync(request, cancellationToken);
        }

        public async Task Handle(IDisposable obj)
        {
            if (obj is HttpRequestMessage request)
            {
                CookieContainer.GetCookies(request.RequestUri)
                    .Cast<Cookie>()
                    .ToList()
                    .ForEach(c => c.Expired = true);
                foreach (var it in _handlers)
                {
                    if (await it.CanHandle(request.RequestUri))
                    {
                        await it.Handle(request);
                    }
                }
            }
        }

        IAsyncAction IScriptHttpInterceptor.Handle(IDisposable message)
        {
            return Handle(message).AsAsyncAction();
        }
    }
}