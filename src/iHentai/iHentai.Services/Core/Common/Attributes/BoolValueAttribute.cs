using System;

namespace iHentai.Services.Core.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class BoolValueAttribute : Attribute, IValueAttribute
    {
        public BoolValueAttribute(string key)
        {
            Key = key;
        }

        public string OnValue { get; set; }
        public string OffValue { get; set; }
        public string Key { get; }
        public string Separator { get; set; } = "=";

        public string GetValue(object instance)
        {
            return (bool) instance ? OnValue : OffValue;
        }

        public string ToString(object instance)
        {
            return $"{Key}{Separator}{GetValue(instance)}";
        }
    }
}