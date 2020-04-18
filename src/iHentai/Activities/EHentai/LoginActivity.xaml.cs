using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using iHentai.Common.Tab;
using iHentai.Services.EHentai;
using iHentai.ViewModels.EHentai;
using Microsoft.Toolkit.Helpers;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Activities.EHentai
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    partial class LoginActivity 
    {
        LoginViewModel ViewModel { get; } = new LoginViewModel();
        public override ITabViewModel TabViewModel => ViewModel;

        public LoginActivity()
        {
            this.InitializeComponent();
        }

        private async void Login()
        {
            await ViewModel.Login();
            if (!Intent.ContainsKey("api"))
            {
                Intent["api"] = ExApi.Instance;
            }
            StartActivity<GalleryActivity>(intent: Intent);
            Finish();
        }
    }
}
