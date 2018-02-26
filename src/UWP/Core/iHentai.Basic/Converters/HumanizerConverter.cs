using System;
using Windows.UI.Xaml.Data;
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
                    return HumanizerDateTime(dateTime);
                case JToken token:
                    if (parameter is string type)
                        if (type == "DateTime")
                            return HumanizerDateTime(token.Value<DateTime>());

                    return token.Value<string>();
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        private static string HumanizerDateTime(DateTime dateTime)
        {
            return dateTime < DateTime.UtcNow - TimeSpan.FromDays(5) ? dateTime.ToOrdinalWords() : dateTime.Humanize();
        }
    }
}