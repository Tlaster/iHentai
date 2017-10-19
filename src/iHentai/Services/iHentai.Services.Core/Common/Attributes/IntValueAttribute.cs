using System;

namespace iHentai.Services.Core.Common.Attributes
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
        public string ToString(object instance)
        {
            if (instance is Enum value)
                return $"{Key}{Separator}{(int)Enum.Parse(value.GetType(), value.ToString())}";
            return $"{Key}{Separator}{instance}";
        }
    }
}