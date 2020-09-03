using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using iHentai.Data;

namespace iHentai.Common
{
    class SettingsManager
    {
        public event EventHandler<ElementTheme>? ThemeChanged; 

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
    }
}
