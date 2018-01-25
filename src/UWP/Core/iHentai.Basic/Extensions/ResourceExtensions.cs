using Windows.ApplicationModel.Resources;
using iHentai.Basic.Helpers;

namespace iHentai.Basic.Extensions
{
    public static class ResourceExtensions
    {
        public static string GetLocalized(this string resourceKey)
        {
            return Singleton<ResourceLoader>.Instance.GetString(resourceKey);
        }
    }
}