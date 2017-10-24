using Newtonsoft.Json;

namespace iHentai.Services.Core.Common
{
    internal static class JsonExtension
    {
        public static string ToJson(this object obj) => JsonConvert.SerializeObject(obj);

        public static object FromJson(this string value) => JsonConvert.DeserializeObject(value);

        public static T FromJson<T>(this string value) => JsonConvert.DeserializeObject<T>(value);
    }
}
