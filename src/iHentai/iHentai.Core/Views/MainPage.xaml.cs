using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iHentai.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        //private void GridSizeChanged(object sender, EventArgs e)
        //{
        //    Debug.WriteLine($"height : {(sender as Grid).Height}");
        //    Debug.WriteLine($"width : {(sender as Grid).Width}");
        //}
    }
}