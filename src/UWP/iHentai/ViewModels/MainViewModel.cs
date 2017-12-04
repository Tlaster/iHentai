using iHentai.Mvvm;
using iHentai.Views;

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
