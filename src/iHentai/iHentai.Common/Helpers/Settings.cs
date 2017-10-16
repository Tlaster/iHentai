// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;

namespace iHentai.Common.Helpers
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	internal static class Settings
	{
		private static ISettings AppSettings => CrossSettings.Current;

	    public static T Get<T>(string key, T defaultValue = default, string fileName = null)
        {
            switch (defaultValue)
            {
                case string value:
                    return (T)(object)AppSettings.GetValueOrDefault(key, value, fileName);
                case bool value:
                    return (T)(object)AppSettings.GetValueOrDefault(key, value, fileName);
                case int value:
                    return (T)(object)AppSettings.GetValueOrDefault(key, value, fileName);
                case long value:
                    return (T)(object)AppSettings.GetValueOrDefault(key, value, fileName);
                case float value:
                    return (T)(object)AppSettings.GetValueOrDefault(key, value, fileName);
                case double value:
                    return (T)(object)AppSettings.GetValueOrDefault(key, value, fileName);
                case decimal value:
                    return (T)(object)AppSettings.GetValueOrDefault(key, value, fileName);
                case DateTime value:
                    return (T)(object)AppSettings.GetValueOrDefault(key, value, fileName);
                default:
                    throw new NotSupportedException();
            }
        }

		public static bool Set<T>(string key, T setValue, string fileName = null)
	    {
	        switch (setValue)
	        {
	            case string value:
	                return AppSettings.AddOrUpdateValue(key, value, fileName);
	            case bool value:
	                return AppSettings.AddOrUpdateValue(key, value, fileName);
	            case int value:
	                return AppSettings.AddOrUpdateValue(key, value, fileName);
	            case long value:
	                return AppSettings.AddOrUpdateValue(key, value, fileName);
	            case float value:
	                return AppSettings.AddOrUpdateValue(key, value, fileName);
	            case double value:
	                return AppSettings.AddOrUpdateValue(key, value, fileName);
	            case decimal value:
	                return AppSettings.AddOrUpdateValue(key, value, fileName);
	            case DateTime value:
	                return AppSettings.AddOrUpdateValue(key, value, fileName);
	            default:
	                throw new NotSupportedException();
            }
	    }

		public static void Remove(string key, string fileName = null)
	    {
	        AppSettings.Remove(key, fileName);
	    }

		public static bool Contains(string key, string fileName = null)
	    {
	        return AppSettings.Contains(key, fileName);
	    }

    }
}