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
using iHentai.ViewModels.Manhuagui;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Activities.Manhuagui
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    partial class ManhuaguiUpdateActivity
    {
        public override ITabViewModel TabViewModel => ViewModel;
        public ManhuaguiUpdateViewModel ViewModel { get; } = new ManhuaguiUpdateViewModel();
        public ManhuaguiUpdateActivity()
        {
            this.InitializeComponent();
        }

        private void AspectRatioView_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenInNewTabClicked(object sender, RoutedEventArgs e)
        {

        }
    }
}
