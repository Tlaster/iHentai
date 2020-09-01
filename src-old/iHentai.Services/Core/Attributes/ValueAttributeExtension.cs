using System.Linq;
using System.Reflection;

namespace iHentai.Services.Core.Attributes
{
    internal static class ValueAttributeExtension
    {
        public static string GetValueAttribute(this MemberInfo memberInfo, object instance)
        {
            return memberInfo.GetCustomAttributesData()
                .Where(attr => typeof(IValueAttribute).IsAssignableFrom(attr.AttributeType))
                .Select(item =>
                    (memberInfo.GetCustomAttribute(item.AttributeType) as IValueAttribute)?.ToString(instance))
                .FirstOrDefault();
        }


        public static IValueAttribute GetAttr(this MemberInfo memberInfo)
        {
            return memberInfo.GetCustomAttributesData()
                .Where(attr => typeof(IValueAttribute).IsAssignableFrom(attr.AttributeType))
                .Select(item => memberInfo.GetCustomAttribute(item.AttributeType) as IValueAttribute)
                .FirstOrDefault();
        }
    }
}