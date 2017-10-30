using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using iHentai.Core;
using iHentai.Platforms.Shared;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace iHentai.Droid
{
    [Activity(Label = "EHTool", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Forms.Init(this, savedInstanceState);
            UserDialogs.Init(() => (Activity) Forms.Context);
            Initialization.Init();
            LoadApplication(new App());
        }
    }
}