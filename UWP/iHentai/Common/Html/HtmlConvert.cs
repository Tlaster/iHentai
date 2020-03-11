using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using iHentai.Common.Helpers;
using iHentai.Common.Html.Attributes;

namespace iHentai.Common.Html
{
    public static class HtmlConvert
    {
        private static HtmlParser GetParser()
        {
            return new HtmlParser();
        }

        public static T DeserializeObject<T>(string html)
        {
            var parser = GetParser();
            var doc = parser.ParseDocument(html);
            return (T) DeserializeObject(doc, typeof(T));
        }

        public static object DeserializeObject(string html, Type type)
        {
            var parser = GetParser();
            var doc = parser.ParseDocument(html);
            return DeserializeObject(doc, type);
        }

        private static object DeserializeObject(IParentNode element, Type type)
        {
            if (!type.IsClass)
            {
                return null;
            }

            var properties =
                type.GetProperties()
                    .Where(item => item.CanWrite && item.CanRead);
            var instance = CreateInstance(type);
            foreach (var propertyInfo in properties)
            {
                var isHtmlItem = Attribute.IsDefined(propertyInfo, typeof(HtmlItemAttribute));
                var isHtmlMultiItems = Attribute.IsDefined(propertyInfo, typeof(HtmlMultiItemsAttribute));
                if (!isHtmlMultiItems && !isHtmlItem)
                {
                    continue;
                }

                object propertyValue = null;

                if (isHtmlItem)
                {
                    propertyValue = DeserializeHtmlItem(propertyInfo, element);
                }
                else if (isHtmlMultiItems)
                {
                    propertyValue = DeserializeHtmlMultiItems(propertyInfo, element);
                }

                if (propertyValue != null)
                {
                    propertyInfo.SetValue(instance, propertyValue);
                }
            }

            return instance;
        }

        private static object CreateInstance(Type type)
        {
            return Expression.Lambda<Func<object>>(
                Expression.New(type.GetConstructor(Type.EmptyTypes))
            ).Compile()();
        }

        private static object DeserializeHtmlMultiItems(PropertyInfo propertyInfo, IParentNode element)
        {
            var attributes = propertyInfo.GetCustomAttributes<HtmlMultiItemsAttribute>().Cast<IHtmlItem>().ToList();
            if (attributes?.Any() != true)
            {
                throw new NullReferenceException();
            }

            var (elements, htmlItem) = GetFirstOfDefaultNodes(element, attributes);
            if (elements == null || htmlItem == null)
            {
                return null;
            }

            var converter = CheckForConverter(propertyInfo);
            var type = propertyInfo.PropertyType;
            var itemType = ReflectionHelper.GetCollectionItemType(type);
            if (itemType == null)
            {
                return null;
            }

            var list = new List<object>();
            foreach (var value in elements)
            {
                var targetValue = GetTargetValue(htmlItem, value, itemType);
                if (converter != null)
                {
                    targetValue = converter.ReadHtml(value, itemType, targetValue);
                }

                if (targetValue != null)
                {
                    list.Add(targetValue);
                }
            }

            var targetEnumerable = typeof(Enumerable)
                .GetMethod("Cast", new[] {typeof(IEnumerable)})
                ?.MakeGenericMethod(itemType)
                .Invoke(null, new object[] {list});
            if (type.IsArray)
            {
                return typeof(Enumerable)
                    .GetMethod("ToArray")
                    ?.MakeGenericMethod(itemType)
                    .Invoke(null, new[] {targetEnumerable});
            }

            if (typeof(List<>).MakeGenericType(itemType) == type)
            {
                return typeof(Enumerable)
                    .GetMethod("ToList")
                    ?.MakeGenericMethod(itemType)
                    .Invoke(null, new[] {targetEnumerable});
            }

            //TODO: More Types
            return null;
        }

        private static object DeserializeHtmlItem(PropertyInfo propertyInfo, IParentNode element)
        {
            var attributes = propertyInfo.GetCustomAttributes<HtmlItemAttribute>().Cast<IHtmlItem>().ToList();
            if (attributes.Any() != true)
            {
                throw new NullReferenceException();
            }

            var (elements, htmlItem) = GetFirstOfDefaultNode(element, attributes);
            if (elements == null || htmlItem == null)
            {
                return null;
            }

            if (htmlItem is HtmlItemAttribute attribute && attribute.RawHtml)
            {
                return elements.InnerHtml;
            }
            var converter = CheckForConverter(propertyInfo);
            var targetValue = GetTargetValue(htmlItem, elements, propertyInfo.PropertyType);
            if (converter != null)
            {
                targetValue = converter.ReadHtml(elements, propertyInfo.PropertyType, targetValue);
            }

            return targetValue;
        }


        private static object GetTargetValue(IHtmlItem htmlItem, IElement element, Type targetType)
        {
            object targetValue = null;
            switch (Type.GetTypeCode(targetType))
            {
                case TypeCode.Boolean:
                case TypeCode.Byte:
                case TypeCode.Char:
                case TypeCode.DateTime:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.String:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    try
                    {
                        targetValue = Convert.ChangeType(GetValue(htmlItem, element), targetType);
                    }
                    catch
                    {
                        // ignored
                    }

                    break;
                case TypeCode.DBNull:
                case TypeCode.Empty:
                    throw new NotSupportedException();
                default:
                    targetValue = DeserializeObject(element, targetType);
                    break;
            }

            return targetValue;
        }

        private static string GetValue(IHtmlItem htmlItem, IElement element)
        {
            var value = (string.IsNullOrEmpty(htmlItem.Attr)
                ? element.Text()
                : element.GetAttribute(htmlItem.Attr))?.Trim();
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            if (!string.IsNullOrEmpty(htmlItem.RegexPattern))
            {
                value = Regex.Match(value, htmlItem.RegexPattern).Groups[htmlItem.RegexGroup].Value;
            }

            return value;
        }

        private static IHtmlConverter CheckForConverter(MemberInfo propertyInfo)
        {
            if (!Attribute.IsDefined(propertyInfo, typeof(HtmlConverterAttribute)))
            {
                return null;
            }

            if (CreateInstance(propertyInfo.GetCustomAttribute<HtmlConverterAttribute>().ConverterType) is
                IHtmlConverter converter)
            {
                return converter;
            }

            return null;
        }

        private static (IElement Element, IHtmlItem HtmlItem) GetFirstOfDefaultNode(IParentNode element,
            IEnumerable<IHtmlItem> attributes)
        {
            IElement node = null;
            IHtmlItem htmlItem = null;
            foreach (var attribute in attributes)
            {
                node = element.QuerySelector(attribute.Selector);
                if (node == null || !string.IsNullOrEmpty(attribute.Attr) && !node.HasAttribute(attribute.Attr))
                {
                    continue;
                }

                htmlItem = attribute;
                break;
            }

            return (node, htmlItem);
        }


        private static (IHtmlCollection<IElement> Elements, IHtmlItem HtmlItem) GetFirstOfDefaultNodes(
            IParentNode element, IEnumerable<IHtmlItem> attributes)
        {
            IHtmlCollection<IElement> node = null;
            IHtmlItem htmlItem = null;
            foreach (var attribute in attributes)
            {
                node = element.QuerySelectorAll(attribute.Selector);
                if (node == null || !node.Any())
                {
                    continue;
                }

                htmlItem = attribute;
                break;
            }

            return (node, htmlItem);
        }
    }

    public interface IHtmlConverter
    {
        object ReadHtml(INode node, Type targetType, object existingValue);
    }
}