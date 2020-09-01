using System;

namespace iHentai.Services.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class EnumValueAttribute : Attribute, IValueAttribute
    {
        public EnumValueAttribute(string key)
        {
            Key = key;
        }

        public string Key { get; }
        public string Separator { get; set; }

        public string GetValue(object instance)
        {
            return instance.GetType().GetField(instance.GetType().GetEnumName(instance)).GetAttr().Key;
        }

        public string ToString(object instance)
        {
            return
                $"{Key}{Separator}{GetValue(instance)}";
        }
    }
}