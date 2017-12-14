using System;
using Windows.UI.Xaml.Data;
using Humanizer;

namespace iHentai.Converters
{
    public class HumanizerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            switch (value)
            {
                case DateTime dateTime:
                    return dateTime.ToOrdinalWords();
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}