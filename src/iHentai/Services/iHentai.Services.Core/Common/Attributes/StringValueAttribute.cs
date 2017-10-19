using System;

namespace iHentai.Services.Core.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    internal sealed class StringValueAttribute : Attribute, IValueAttribute
    {
        public StringValueAttribute(string key)
        {
            Key = key;
        }

        public string Key { get; }
        public string Separator { get; set; } = "=";
        public string ToString(object instance)
        {
            return $"{Key}{Separator}{instance}";
        }
    }
}