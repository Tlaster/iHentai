using System;

namespace iHentai.Services.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class IntValueAttribute : Attribute, IValueAttribute
    {
        public IntValueAttribute(string key)
        {
            Key = key;
        }

        public string Key { get; }
        public string Separator { get; set; } = "=";

        public string GetValue(object instance)
        {
            if (instance is Enum value)
            {
                return ((int) Enum.Parse(value.GetType(), value.ToString())).ToString();
            }

            return instance + "";
        }

        public string ToString(object instance)
        {
            return $"{Key}{Separator}{GetValue(instance)}";
        }
    }
}