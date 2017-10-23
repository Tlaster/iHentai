using iHentai.Core.ViewModels;
using iHentai.Services.Core;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iHentai.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage(IHentaiApis apis)
        {
            InitializeComponent();
            BindingContext = new MainViewModel(apis);
        }
    }
}