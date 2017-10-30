using System;
using Plugin.Settings;

namespace iHentai.Core.Common.Helpers
{
    public static class Settings
    {
        public static T Get<T>(string key, T defaultValue = default)
        {
            switch (defaultValue)
            {
                case string value:
                    return (T) (object) CrossSettings.Current.GetValueOrDefault(key, value, "iHentai");
                case bool value:
                    return (T) (object) CrossSettings.Current.GetValueOrDefault(key, value, "iHentai");
                case int value:
                    return (T) (object) CrossSettings.Current.GetValueOrDefault(key, value, "iHentai");
                case long value:
                    return (T) (object) CrossSettings.Current.GetValueOrDefault(key, value, "iHentai");
                case float value:
                    return (T) (object) CrossSettings.Current.GetValueOrDefault(key, value, "iHentai");
                case double value:
                    return (T) (object) CrossSettings.Current.GetValueOrDefault(key, value, "iHentai");
                case decimal value:
                    return (T) (object) CrossSettings.Current.GetValueOrDefault(key, value, "iHentai");
                case DateTime value:
                    return (T) (object) CrossSettings.Current.GetValueOrDefault(key, value, "iHentai");
                case Enum value:
                    return (T) Enum.Parse(typeof(T),
                        CrossSettings.Current.GetValueOrDefault(key, Enum.GetName(typeof(T), value), "iHentai"));
                default:
                    throw new NotSupportedException();
            }
        }

        public static bool Set<T>(string key, T setValue)
        {
            switch (setValue)
            {
                case string value:
                    return CrossSettings.Current.AddOrUpdateValue(key, value, "iHentai");
                case bool value:
                    return CrossSettings.Current.AddOrUpdateValue(key, value, "iHentai");
                case int value:
                    return CrossSettings.Current.AddOrUpdateValue(key, value, "iHentai");
                case long value:
                    return CrossSettings.Current.AddOrUpdateValue(key, value, "iHentai");
                case float value:
                    return CrossSettings.Current.AddOrUpdateValue(key, value, "iHentai");
                case double value:
                    return CrossSettings.Current.AddOrUpdateValue(key, value, "iHentai");
                case decimal value:
                    return CrossSettings.Current.AddOrUpdateValue(key, value, "iHentai");
                case DateTime value:
                    return CrossSettings.Current.AddOrUpdateValue(key, value, "iHentai");
                case Enum value:
                    return CrossSettings.Current.AddOrUpdateValue(key, Enum.GetName(typeof(T), value), "iHentai");
                default:
                    throw new NotSupportedException();
            }
        }

        public static void Remove(string key)
        {
            CrossSettings.Current.Remove(key, "iHentai");
        }

        public static bool Contains(string key)
        {
            return CrossSettings.Current.Contains(key, "iHentai");
        }
    }
}