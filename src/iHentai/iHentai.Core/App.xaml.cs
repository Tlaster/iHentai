using iHentai.Core.ViewModels;
using iHentai.Core.Views;
using iHentai.Mvvm;
using Xamarin.Forms;

namespace iHentai.Core
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Plugin.Iconize.Iconize.With(new Plugin.Iconize.Fonts.MaterialModule());
            MainPage = new MvvmNavigationPage(new GalleryPage());
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