using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using iHentai.Extensions.Models;
using Microsoft.UI.Xaml.Controls;

namespace iHentai.Common.Converters
{
    class ExtensionManifestToNavigationViewItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is ExtensionManifest item)
            {
                return new NavigationViewItem
                {
                    Tag = item,
                    Content = item.Name,
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
