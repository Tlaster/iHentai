using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace iHentai.Helpers
{
    internal static class ThemeHelper
    {
        public static void AccentColorUpdated(FrameworkElement elementWithText)
        {
            elementWithText.RequestedTheme = CheckColorIsDark(new UISettings().GetColorValue(UIColorType.Accent)) ? ElementTheme.Light : ElementTheme.Dark;
        }

        private static bool CheckColorIsDark(Color c)
        {
            return 5 * c.G + 2 * c.R + c.B <= 8 * 128;
        }
    }
}