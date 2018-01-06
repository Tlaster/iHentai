using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using iHentai.Basic.Extensions;
using iHentai.Basic.Helpers;
using iHentai.Mvvm;
using iHentai.Services;

namespace iHentai.ViewModels
{
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

        public bool EnableHentaiMode
        {
            get => "enableHentaiMode".Read(false);
            set
            {
                value.Save("enableHentaiMode");
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
                CoreApplication.RequestRestartAsync(string.Empty);
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
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