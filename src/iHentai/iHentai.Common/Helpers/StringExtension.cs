using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace iHentai.Common.Helpers
{
    internal static class StringExtension
    {
        private const string ResourceId = "iHentai.Core.Resources.AppResources";

        private static readonly Lazy<ResourceManager> ResMgr = new Lazy<ResourceManager>(() =>
            new ResourceManager(ResourceId
                , typeof(StringExtension).GetTypeInfo().Assembly));

        public static string ToLocalized(this string key)
        {
            if (key == null)
                return "";
            var translation = ResMgr.Value.GetString(key, CultureInfo.CurrentCulture) ?? key;
            return translation;
        }

        public static IEnumerable<string> ToLocalized(this IEnumerable<string> keys)
        {
            if (keys == null || !keys.Any())
                return new string[0];
            return keys.Select(key => key.ToLocalized()).ToList();
        }
    }
}