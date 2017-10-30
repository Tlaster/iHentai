using System.Linq;
using System.Reflection;

namespace iHentai.Services.Core.Common.Attributes
{
    internal interface IValueAttribute
    {
        string Key { get; }
        string Separator { get; set; }
        string ToString(object instance);
        string GetValue(object instance);
    }

    internal static class ValueAttributeExtension
    {
        public static string Get(this MemberInfo memberInfo, object instance)
        {
            return memberInfo.GetCustomAttributesData()
                .Where(attr => typeof(IValueAttribute).IsAssignableFrom(attr.AttributeType))
                .Select(item =>
                    (memberInfo.GetCustomAttribute(item.AttributeType) as IValueAttribute)?.ToString(instance))
                .FirstOrDefault();
        }


        public static IValueAttribute GetAttr(this MemberInfo memberInfo, object instance)
        {
            return memberInfo.GetCustomAttributesData()
                .Where(attr => typeof(IValueAttribute).IsAssignableFrom(attr.AttributeType))
                .Select(item => memberInfo.GetCustomAttribute(item.AttributeType) as IValueAttribute)
                .FirstOrDefault();
        }
    }
}