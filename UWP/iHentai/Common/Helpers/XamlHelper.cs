using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace iHentai.Common.Helpers
{
    static class XamlHelper
    {
        public static Visibility IsEmptyToVisibility(object any)
        {
            if (any is string stringValue)
            {
                return BoolToVisibility(string.IsNullOrEmpty(stringValue));
            }

            if (any is IEnumerable enumerable)
            {
                return BoolToVisibility(!enumerable.OfType<object>().Any());
            }

            return BoolToVisibility(false);
        }

        public static Visibility BoolToVisibility(bool visible)
        {
            return visible ? Visibility.Visible : Visibility.Collapsed;
        }
        public static Visibility InvertBoolToVisibility(bool collapsed)
        {
            return BoolToVisibility(!collapsed);
        }
    }
}
