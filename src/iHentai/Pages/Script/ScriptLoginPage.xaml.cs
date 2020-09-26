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
using iHentai.Services.Models.Script;
using iHentai.ViewModels.Script;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Pages.Script
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ScriptLoginPage : Page
    {
        private ScriptLoginViewModel ViewModel { get; set; }
        public ScriptLoginPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is ScriptLoginViewModel viewModel)
            {
                ViewModel = viewModel;
            }
        }
        private async void Login()
        {
            var result = await ViewModel.Login();
            if (result == 200)
            {
                Frame.Navigate(typeof(ScriptGalleryPage), new ScriptGalleryViewModel(ViewModel.Api));
                Frame.BackStack.Clear();
            }
            else
            {
                //TODO: show login error
            }
        }
    }
}
