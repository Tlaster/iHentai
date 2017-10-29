﻿using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.Widget;
using Android.OS;
using FFImageLoading.Forms.Droid;
using Xamarin.Forms;
using iHentai.Platforms.Shared;

namespace iHentai.Droid
{
    [Activity(Label = "EHTool", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            UserDialogs.Init(() => (Activity)Forms.Context);
            Initialization.Init();
            LoadApplication(new Core.App());
        }
    }
}

