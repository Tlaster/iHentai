using Newtonsoft.Json;

namespace iHentai.Apis.Core.Common
{
    internal static class JsonExtension
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static object FromJson(this string value)
        {
            return JsonConvert.DeserializeObject(value);
        }

        public static T FromJson<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}