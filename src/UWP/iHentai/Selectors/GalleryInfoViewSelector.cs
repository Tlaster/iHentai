using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using iHentai.Apis.Core.Models.Interfaces;
using iHentai.Helpers;

namespace iHentai.Selectors
{
    public class GalleryInfoViewSelector : DataTemplateSelector
    {
        public GalleryInfoViewSelector()
        {
            KnownViews = typeof(IGalleryInfoView<>).GetTypeInfo().Assembly.DefinedTypes
                .Select(item =>
                    item.IsClass &&
                    ReflectionHelper.ImplementsGenericDefinition(item, typeof(IGalleryInfoView<>), out var res)
                        ? new {ViewType = item, GenericType = res.GetGenericArguments()[0]}
                        : null).Where(item => item != null)
                .ToDictionary(item => item.GenericType, item => item.ViewType);
        }

        public Dictionary<Type, TypeInfo> KnownViews { get; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (!(item is IGalleryModel))
                return new DataTemplate();
            if (KnownViews.TryGetValue(item.GetType(), out var type) &&
                Application.Current.Resources.TryGetValue(type.FullName, out var res))
                return res as DataTemplate;
            return new DataTemplate();
        }
    }
}