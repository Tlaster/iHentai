using System;
using System.IO;
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
                SettingsDb.Instance.Get("extension_folder", null) ?? JsonConvert.SerializeObject(new ExtensionFolderData
                    {Path = Path.Combine(Environment.CurrentDirectory, "Extensions")}));
            set
            {
                SettingsDb.Instance.Set("extension_folder", JsonConvert.SerializeObject(value));
                this.Resolve<IExtensionManager>().Reload();
            }
        }

        public event EventHandler<ElementTheme>? ThemeChanged;
    }
}