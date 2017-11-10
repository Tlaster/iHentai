using iHentai.Core.ViewModels;
using iHentai.Core.Views;
using iHentai.Mvvm;
using Plugin.Iconize;
using Plugin.Iconize.Fonts;
using Xamarin.Forms;

namespace iHentai.Core
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Iconize.With(new MaterialModule());
            MainPage = new MvvmNavigationPage(new GalleryPage
            {
                BindingContext = new GalleryViewModel()
            });
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}