using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Newtonsoft.Json.Linq;

namespace iHentai.Basic.Converters
{
    public class EqualsConverter : IValueConverter
    {
        public bool Invert { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool result;
            switch (value)
            {
                case JValue token:
                    result = System.Convert.ChangeType(token.Value, parameter.GetType()).Equals(parameter);
                    break;
                default:
                    result = value?.Equals(parameter) ?? false;
                    break;
            }

            result = Invert ? !result : result;

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