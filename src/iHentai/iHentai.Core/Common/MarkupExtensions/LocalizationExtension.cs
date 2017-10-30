using System;
using iHentai.Core.Common.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iHentai.Core.Common.MarkupExtensions
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