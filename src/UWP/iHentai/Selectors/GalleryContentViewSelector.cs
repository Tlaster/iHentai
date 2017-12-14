using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using iHentai.Apis.Core.Models.Interfaces;
using iHentai.Basic.Helpers;

namespace iHentai.Selectors
{
    public class GalleryContentViewSelector : DataTemplateSelector
    {
        private static readonly Dictionary<(Type ItemType, ContentTypes ContentType), TypeInfo> KnownViews;

        static GalleryContentViewSelector()
        {
            KnownViews = typeof(IGalleryContentView<>).GetTypeInfo().Assembly.DefinedTypes
                .Select(item =>
                    item.IsClass && item.GetCustomAttribute<ContentTypeAttribute>()?.ContentType != null &&
                    ReflectionHelper.ImplementsGenericDefinition(item, typeof(IGalleryContentView<>), out var res)
                        ? new {ViewType = item, GenericType = res.GetGenericArguments()[0]}
                        : null).Where(item => item != null)
                .ToDictionary(
                    item => (ItemType: item.GenericType, item.ViewType.GetTypeInfo()
                        .GetCustomAttribute<ContentTypeAttribute>().ContentType), item => item.ViewType);
        }

        public ContentTypes ContentType { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item == null || !KnownViews.TryGetValue((item.GetType(), ContentType), out var type))
                return new DataTemplate();
            var template =
                $"<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><views:{type.Name} xmlns:views=\"using:{type.Namespace}\"/></DataTemplate>";
            return (DataTemplate) XamlReader.Load(template);
        }
    }
}