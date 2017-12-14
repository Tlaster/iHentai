using System;
using Windows.UI.Xaml.Data;
using Html2Markdown;

namespace iHentai.Basic.Converters
{
    public class HtmlToMarkdownConverter : IValueConverter
    {
        private static readonly Converter _converter = new Converter();
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return _converter.Convert(value + "");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
