using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using iHentai.Helpers;
using iHentai.Mvvm;
using iHentai.Services;
using iHentai.Views;

namespace iHentai.ViewModels
{
    [Page(typeof(SettingsPage))]
    public class SettingsViewModel : ViewModel
    {
        public ElementTheme ElementTheme { get; set; } = ThemeSelectorService.Theme;

        public string VersionDescription
        {
            get
            {
                var package = Package.Current;
                var packageId = package.Id;
                var version = packageId.Version;
                return $"{package.DisplayName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            }
        }

        public ICommand SwitchThemeCommand => new RelayCommand<ElementTheme>(
            param =>
            {
                ElementTheme = param;
                ThemeSelectorService.SetTheme(param);
            });
    }
}