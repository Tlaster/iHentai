using System;
using Windows.UI.Xaml.Data;
using iHentai.Extensions.Models;
using Microsoft.UI.Xaml.Controls;

namespace iHentai.Common.Converters
{
    internal class ExtensionManifestToNavigationViewItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is ExtensionManifest item)
            {
                return new NavigationViewItem
                {
                    Tag = item,
                    Content = item.Name
                };
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}