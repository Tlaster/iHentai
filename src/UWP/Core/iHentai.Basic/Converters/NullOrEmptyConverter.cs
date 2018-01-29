using System;
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
            var result = value.IsEmpty() || value is JValue token && token.Value.IsEmpty()
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