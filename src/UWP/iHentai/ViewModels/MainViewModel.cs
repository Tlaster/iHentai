using iHentai.Mvvm;
using iHentai.Views;

namespace iHentai.ViewModels
{
    [Page(typeof(MainPage))]
    public class MainViewModel : ViewModel
    {
        public void GoSettings()
        {
            Navigate<SettingsViewModel>();
        }
    }
}
