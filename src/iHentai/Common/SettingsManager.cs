using System;
using System.IO;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using iHentai.Data;
using iHentai.Extensions;
using iHentai.Extensions.Models;
using Newtonsoft.Json;

namespace iHentai.Common
{
    internal class SettingsManager
    {
        public static SettingsManager Instance { get; } = new SettingsManager();

        public ElementTheme Theme
        {
            get => Enum.Parse<ElementTheme>(SettingsDb.Instance.Get("app_theme", ElementTheme.Default.ToString()));
            set
            {
                SettingsDb.Instance.Set("app_theme", value.ToString());
                ThemeChanged?.Invoke(this, value);
            }
        }

        public ExtensionFolderData ExtensionFolder
        {
            get => JsonConvert.DeserializeObject<ExtensionFolderData>(
                SettingsDb.Instance.Get<string?>("extension_folder") ?? JsonConvert.SerializeObject(new ExtensionFolderData
                    {Path = Path.Combine(Environment.CurrentDirectory, "Extensions")}));
            set
            {
                SettingsDb.Instance.Set("extension_folder", JsonConvert.SerializeObject(value));
                this.Resolve<IExtensionManager>().Reload();
            }
        }

        public bool EnableExtension
        {
            get => SettingsDb.Instance.Get("enable_extension", false);
            set => SettingsDb.Instance.Set("enable_extension", value);
        }

        public bool UseLocalExtension
        {
            get => SettingsDb.Instance.Get("use_local_extension", false);
            set
            {
                SettingsDb.Instance.Set("use_local_extension", value);
                CoreApplication.RequestRestartAsync(string.Empty);
            }
        }

        public event EventHandler<ElementTheme>? ThemeChanged;
    }
}