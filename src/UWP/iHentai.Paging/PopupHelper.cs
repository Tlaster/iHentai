using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Microsoft.Toolkit.Uwp.UI.Extensions;

namespace iHentai.Paging
{
    public static class PopupHelper
    {
        public static bool IsPopupVisible
        {
            get
            {
                return VisualTreeHelper.GetOpenPopups(Window.Current).Any(p => p.ActualWidth > 0 && p.ActualHeight > 0);
            }
        }

        public static Popup GetParentPopup(FrameworkElement element)
        {
            return element.FindAscendant<Popup>();
        }

        public static bool IsInPopup(FrameworkElement element)
        {
            if (element is Popup)
                return true;

            return GetParentPopup(element) != null;
        }
    }
}