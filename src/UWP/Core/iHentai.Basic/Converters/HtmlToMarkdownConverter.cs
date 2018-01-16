using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Data;
using Html2Markdown;
using Html2Markdown.Replacement;
using Html2Markdown.Scheme;
using iHentai.Basic.Helpers;

namespace iHentai.Basic.Converters
{
    public class HtmlToMarkdownConverter : IValueConverter
    {
        private static Converter Converter => Singleton<Converter>.Instance;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return Converter.Convert(value + "");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}