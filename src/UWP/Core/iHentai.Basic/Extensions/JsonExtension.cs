using System;
using Newtonsoft.Json;

namespace iHentai.Basic.Extensions
{
    public static class JsonExtension
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static object JsonToObject(this string value)
        {
            return JsonConvert.DeserializeObject(value);
        }

        public static object JsonToObject(this string value, Type type)
        {
            return JsonConvert.DeserializeObject(value, type);
        }

        public static T JsonTo<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}