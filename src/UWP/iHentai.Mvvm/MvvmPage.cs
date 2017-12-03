using iHentai.Paging;

namespace iHentai.Mvvm
{
    public class MvvmPage : HentaiPage
    {
        public ViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(HentaiNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel = e.Parameter as ViewModel;
            DataContext = ViewModel;
        }
    }
}