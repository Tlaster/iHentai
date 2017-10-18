using System;

namespace iHentai.Services.Core.Common
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    internal sealed class IntValueAttribute : Attribute, IValueAttribute
    {
        public IntValueAttribute(string key)
        {
            Key = key;
        }

        public string Key { get; }
        public string Separator { get; set; } = "=";
        public string ToQueryString(object instance)
        {
            return $"{Key}{Separator}{instance}";
        }
    }
}