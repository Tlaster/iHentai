using System;
using System.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using iHentai.Basic.Extensions;
using Newtonsoft.Json.Linq;

namespace iHentai.Basic.Converters
{
    public class NullOrEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var isInverted = false;
            if (parameter is string) bool.TryParse(parameter.ToString(), out isInverted);
            var result = value == null || int.TryParse(value.ToString(), out var res) && res == 0 ||
                         decimal.TryParse(value + "", out var deres) && deres == 0M ||
                         value is string && string.IsNullOrEmpty(value.ToString()) ||
                         value is IList list && list.Count == 0 ||
                         value is JArray array && array.Count == 0 ||
                         value is JToken token && !(value is JArray) && token.Value<string>().IsEmpty()
                ? isInverted
                : !isInverted;
            if (targetType == typeof(bool))
                return result;
            if (targetType == typeof(Visibility))
                return result ? Visibility.Visible : Visibility.Collapsed;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}