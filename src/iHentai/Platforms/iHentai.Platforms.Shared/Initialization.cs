#if WINDOWS_UWP
using FFImageLoading.Forms.WinUWP;
#elif __IOS__
using FFImageLoading.Forms.Touch;
#elif ANDROID
using FFImageLoading.Forms.Droid;
#elif __MACOS__
using FFImageLoading.Forms.Mac;
#endif


namespace iHentai.Platforms.Shared
{
    public static class Initialization
    {
        public static void Init()
        {
            CachedImageRenderer.Init();
        }
    }
}