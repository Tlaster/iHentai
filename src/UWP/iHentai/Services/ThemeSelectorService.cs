using System;
using Windows.UI.Xaml;
using iHentai.Helpers;

namespace iHentai.Services
{
    public static class ThemeSelectorService
    {
        private const string SettingsKey = "RequestedTheme";

        public static ElementTheme Theme { get; set; } = ElementTheme.Default;

        public static void Initialize()
        {
            Theme = LoadThemeFromSettings();
        }

        public static void SetTheme(ElementTheme theme)
        {
            Theme = theme;
            SetRequestedTheme();
            SaveThemeInSettings(Theme);
        }

        public static void SetRequestedTheme()
        {
            if (Window.Current.Content is FrameworkElement frameworkElement)
                frameworkElement.RequestedTheme = Theme;
        }

        private static ElementTheme LoadThemeFromSettings()
        {
            var cacheTheme = ElementTheme.Default;
            var themeName = SettingsKey.Read<string>();

            if (!string.IsNullOrEmpty(themeName))
                Enum.TryParse(themeName, out cacheTheme);

            return cacheTheme;
        }

        private static void SaveThemeInSettings(ElementTheme theme)
        {
            theme.Save(SettingsKey);
        }
    }
}