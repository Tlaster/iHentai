using System;

namespace iHentai.Services.Core.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class EnumValueAttribute : Attribute, IValueAttribute
    {
        public EnumValueAttribute(string key)
        {
            Key = key;
        }

        public string Key { get; }
        public string Separator { get; set; }
        public string ToString(object instance)
        {
            return
                $"{Key}{Separator}{instance.GetType().GetField(instance.GetType().GetEnumName(instance)).GetAttr(instance).Key}";
        }
    }
}