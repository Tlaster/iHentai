using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using iHentai.Common.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iHentai.Common.MarkupExtensions
{
    [ContentProperty(nameof(Text))]
    public class LocalizationExtension : IMarkupExtension
    {
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return Text.ToLocalized();
        }

    }
}
