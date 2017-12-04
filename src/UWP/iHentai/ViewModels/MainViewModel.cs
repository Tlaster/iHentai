using iHentai.Mvvm;

namespace iHentai.ViewModels
{
    public class MainViewModel : ViewModel
    {
        public void GoSettings()
        {
            Navigate<SettingsViewModel>();
        }
    }
}