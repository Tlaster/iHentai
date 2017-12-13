using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Data;
using iHentai.Apis.NHentai.Models;

namespace iHentai.Apis.NHentai.Converters
{
    public class TagConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var item = value as TagModel[];
            return item.GroupBy(v => v.Type).Select(v => new {Title = v.Key, Tags = v.Select(x => x.Name).ToArray()});
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
