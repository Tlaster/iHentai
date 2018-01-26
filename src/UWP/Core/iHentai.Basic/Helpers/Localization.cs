using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Markup;
using iHentai.Basic.Extensions;

namespace iHentai.Basic.Helpers
{
    public class Localization : MarkupExtension
    {
        public string Key { get; set; }

        protected override object ProvideValue()
        {
            return Key.GetLocalized();
        }
    }
}
