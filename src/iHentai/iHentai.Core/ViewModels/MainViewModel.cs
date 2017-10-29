using iHentai.Core.Views;
using iHentai.Mvvm;
using iHentai.Services.Core;

namespace iHentai.Core.ViewModels
{
    [Page(typeof(MainPage))]
    public class MainViewModel : ViewModel
    {
        private readonly IHentaiApis _apis;

        public MainViewModel(IHentaiApis apis)
        {
            _apis = apis;
        }
    }
}