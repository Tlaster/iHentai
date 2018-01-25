using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using iHentai.Basic.Helpers;

namespace iHentai.Services
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ContentKeyAttribute : Attribute
    {
        public ContentKeyAttribute(string key)
        {
            Key = key;
        }

        public string Key { get; }
    }

    public class ContentViewTemplateSelector : DataTemplateSelector
    {
        private static readonly Dictionary<(Type ItemType, string ContentType), TypeInfo> KnownViews;

        static ContentViewTemplateSelector()
        {
            KnownViews = (Application.Current as IMultiContentApplication)?.GetContentViewAssemblies()
                .SelectMany(x => x.DefinedTypes)
                .Select(item =>
                    item.IsClass &&
                    ReflectionHelper.ImplementsGenericDefinition(item, typeof(IContentView<>), out var res)
                        ? new {ViewType = item, GenericType = res.GetGenericArguments()[0]}
                        : null).Where(item => item != null)
                .ToDictionary(
                    item => (ItemType: item.GenericType, item.ViewType.GetTypeInfo()
                        .GetCustomAttribute<ContentKeyAttribute>()?.Key), item => item.ViewType);
        }

        public string ContentKey { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item == null)
                return new DataTemplate();
            var items = KnownViews.Where(x => x.Key.ItemType == item.GetType());
            if (!items.Any()) return new DataTemplate();
            if (items.Count() > 1) items = items.Where(x => x.Key.ContentType == ContentKey);
            var type = items.FirstOrDefault().Value;
            var template =
                $"<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><views:{type.Name} xmlns:views=\"using:{type.Namespace}\"/></DataTemplate>";
            return XamlReader.Load(template) as DataTemplate;
        }
    }

    public interface IMultiContentApplication
    {
        IEnumerable<Assembly> GetContentViewAssemblies();
    }

    public interface IContentView<T>
    {
    }
}