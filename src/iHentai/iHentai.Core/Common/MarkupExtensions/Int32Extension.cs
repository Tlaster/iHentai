using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iHentai.Core.Common.MarkupExtensions
{
    [ContentProperty(nameof(Value))]
    public sealed class Int32Extension : IMarkupExtension<int>
    {
        public string Value { get; set; }
        public int ProvideValue(IServiceProvider serviceProvider)
        {
            return int.Parse(Value);
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }
    }

    [ContentProperty(nameof(Value))]
    public sealed class BoolExtension : IMarkupExtension<bool>
    {
        public string Value { get; set; }
        public bool ProvideValue(IServiceProvider serviceProvider)
        {
            return bool.Parse(Value);
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }
    }
}
