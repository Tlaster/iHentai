using Windows.ApplicationModel.Resources;
using iHentai.Basic.Helpers;

namespace iHentai.Basic.Extensions
{
    public static class ResourceExtensions
    {
        public static string GetLocalized(this string resourceKey)
        {
            var result = Singleton<ResourceLoader>.Instance.GetString(resourceKey);
            return result.IsEmpty() ? resourceKey : result;
        }
    }
}