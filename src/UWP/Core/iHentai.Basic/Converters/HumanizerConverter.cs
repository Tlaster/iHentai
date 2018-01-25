using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Humanizer;
using Newtonsoft.Json.Linq;

namespace iHentai.Basic.Converters
{

    public class HumanizerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            switch (value)
            {
                case DateTime dateTime:
                    return dateTime.ToOrdinalWords();
                case JToken token:
                    if (parameter is string type)
                        if (type == "DateTime")
                            return token.Value<DateTime>().ToOrdinalWords();

                    return token.Value<string>();
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