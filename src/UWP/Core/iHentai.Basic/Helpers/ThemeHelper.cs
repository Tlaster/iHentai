using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace iHentai.Basic.Helpers
{
    public static class ThemeHelper
    {
        public static void AccentColorUpdated(FrameworkElement elementWithText)
        {
            elementWithText.RequestedTheme = CheckColorIsLight(new UISettings().GetColorValue(UIColorType.Accent))
                ? ElementTheme.Light
                : ElementTheme.Dark;
        }

        public static bool CheckColorIsLight(Color c)
        {
            return 5 * c.G + 2 * c.R + c.B <= 8 * 128;
        }
    }
}