using System;
using Plugin.Settings;

namespace iHentai.Services.Core.Common
{
    internal class Settings : ISettings
    {
        public Settings(string fileName)
        {
            FileName = fileName;
        }

        public string FileName { get; }

        public T Get<T>(string key, T defaultValue = default)
        {
            switch (defaultValue)
            {
                case string value:
                    return (T) (object) CrossSettings.Current.GetValueOrDefault(key, value, FileName);
                case bool value:
                    return (T) (object) CrossSettings.Current.GetValueOrDefault(key, value, FileName);
                case int value:
                    return (T) (object) CrossSettings.Current.GetValueOrDefault(key, value, FileName);
                case long value:
                    return (T) (object) CrossSettings.Current.GetValueOrDefault(key, value, FileName);
                case float value:
                    return (T) (object) CrossSettings.Current.GetValueOrDefault(key, value, FileName);
                case double value:
                    return (T) (object) CrossSettings.Current.GetValueOrDefault(key, value, FileName);
                case decimal value:
                    return (T) (object) CrossSettings.Current.GetValueOrDefault(key, value, FileName);
                case DateTime value:
                    return (T) (object) CrossSettings.Current.GetValueOrDefault(key, value, FileName);
                case Enum value:
                    return (T) Enum.Parse(typeof(T),
                        CrossSettings.Current.GetValueOrDefault(key, Enum.GetName(typeof(T), value), FileName));
                default:
                    return (T) (object) CrossSettings.Current.GetValueOrDefault(key, defaultValue.ToJson(), FileName);
            }
        }

        public bool Set<T>(string key, T setValue)
        {
            switch (setValue)
            {
                case string value:
                    return CrossSettings.Current.AddOrUpdateValue(key, value, FileName);
                case bool value:
                    return CrossSettings.Current.AddOrUpdateValue(key, value, FileName);
                case int value:
                    return CrossSettings.Current.AddOrUpdateValue(key, value, FileName);
                case long value:
                    return CrossSettings.Current.AddOrUpdateValue(key, value, FileName);
                case float value:
                    return CrossSettings.Current.AddOrUpdateValue(key, value, FileName);
                case double value:
                    return CrossSettings.Current.AddOrUpdateValue(key, value, FileName);
                case decimal value:
                    return CrossSettings.Current.AddOrUpdateValue(key, value, FileName);
                case DateTime value:
                    return CrossSettings.Current.AddOrUpdateValue(key, value, FileName);
                case Enum value:
                    return CrossSettings.Current.AddOrUpdateValue(key, Enum.GetName(typeof(T), value), FileName);
                default:
                    return CrossSettings.Current.AddOrUpdateValue(key, setValue.ToJson(), FileName);
            }
        }

        public void Remove(string key)
        {
            CrossSettings.Current.Remove(key, FileName);
        }

        public bool Contains(string key)
        {
            return CrossSettings.Current.Contains(key, FileName);
        }
    }
}