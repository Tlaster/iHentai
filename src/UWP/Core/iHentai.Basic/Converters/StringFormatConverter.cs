using System;
using Windows.UI.Xaml.Data;

namespace iHentai.Basic.Converters
{
    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return null;

            if (parameter == null) return value;

            return string.Format(parameter + "", value);
        }


        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}