#if WINDOWS_UWP
using FFImageLoading.Forms.WinUWP;
#elif __IOS__
using FFImageLoading.Forms.Touch;
#elif ANDROID
using FFImageLoading.Forms.Droid;
#elif __MACOS__
using FFImageLoading.Forms.Mac;
#endif
using FFImageLoading.Config;
using iHentai.Core.Common;
using FFImageLoading;
using System.Net.Http;

namespace iHentai.Platforms.Shared
{
    public static class Initialization
    {
        public static void Init()
        {
            CachedImageRenderer.Init();
            ImageService.Instance.Initialize(new Configuration
            {
                HttpClient = new HttpClient(new HentaiHttpClient())
            });
        }
    }
}