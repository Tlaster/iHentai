using Newtonsoft.Json;

namespace iHentai.Helpers
{
    internal static class JsonExtension
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        
        public static object JsonToObject(this string value)
        {
            return JsonConvert.DeserializeObject(value);
        }

        public static T JsonTo<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}