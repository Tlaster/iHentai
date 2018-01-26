using Conet.ViewModels;
using iHentai.Mvvm;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Conet.Pages
{
    /// <summary>
    ///     可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HomePage : IMvvmView<HomeViewModel>
    {
        public HomePage()
        {
            InitializeComponent();
        }

        public new HomeViewModel ViewModel
        {
            get => (HomeViewModel) base.ViewModel;
            set => base.ViewModel = value;
        }
    }
}