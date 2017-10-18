using System.Linq;
using System.Reflection;

namespace iHentai.Services.Core.Common
{
    public abstract class AutoString
    {
        
        protected abstract string Separator { get; }
        
        public override string ToString()
        {
            var values = GetType()
                .GetProperties()
                .Select(property => (Attribute: property
                    .GetCustomAttributesData()
                    .FirstOrDefault(attr => typeof(IValueAttribute).IsAssignableFrom(attr.AttributeType)), Property : property)
                )
                .Where(item => item.Attribute != null)
                .Select(item => (item.Property.GetCustomAttribute(item.Attribute.AttributeType) as IValueAttribute)?.ToQueryString(item.Property.GetValue(this)))
                .Where(item => !string.IsNullOrEmpty(item))
                .ToList();
            return string.Join(Separator, values);
        }
    }
}