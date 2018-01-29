using System.Collections;

namespace iHentai.Basic.Extensions
{
    public static class EmptyExtension
    {
        public static bool IsEmpty<T>(this T value)
        {
            if (value == null) return true;
            switch (value)
            {
                case string str:
                    return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
                case ICollection collection:
                    return collection.Count == 0;
                default:
                    return value.Equals(default(T));
            }
        }

        public static bool IsEmpty(this object value)
        {
            if (value == null) return true;
            switch (value)
            {
                case string str:
                    return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
                case ICollection collection:
                    return collection.Count == 0;
                case int intValue:
                    return intValue == default;
                case double doubleValue:
                    return doubleValue == default;
                case float floatValue:
                    return floatValue == default;
                case long longValue:
                    return longValue == default;
                case decimal decimalValue:
                    return decimalValue == default;
                default:
                    return value.Equals(default);
            }
        }
    }
}