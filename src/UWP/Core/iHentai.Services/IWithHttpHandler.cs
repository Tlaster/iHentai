using System.Net.Http;

namespace iHentai.Services
{
    public interface IWithHttpHandler
    {
        bool CanHandle(HttpRequestMessage message);
        void Handle(ref HttpRequestMessage message);
    }
}