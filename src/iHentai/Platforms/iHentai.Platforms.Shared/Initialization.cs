#if WINDOWS_UWP
using FFImageLoading.Forms.WinUWP;
#elif __IOS__
using FFImageLoading.Forms.Touch;
#elif ANDROID
using System.Net.Http;
using Xamarin.Android.Net;
using FFImageLoading.Forms.Droid;
#elif __MACOS__
using FFImageLoading.Forms.Mac;
#endif
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FFImageLoading;
using FFImageLoading.Config;
using iHentai.Core.Common;

namespace iHentai.Platforms.Shared
{
    public static class Initialization
    {
        public static void Init()
        {
            CachedImageRenderer.Init();
            ImageService.Instance.Initialize(new Configuration
            {
#if ANDROID
                HttpClient = new HttpClient(new HentaiAndroidHttpClientHandler())
#else
                HttpClient = new HttpClient(new HentaiHttpClient())
#endif
            });
        }
    }

#if ANDROID
    internal class HentaiAndroidHttpClientHandler : AndroidClientHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HentaiHttpHandler.AddHeader(request);
            return base.SendAsync(request, cancellationToken);
        }
    }
#endif
}