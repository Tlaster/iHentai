using System;
using System.Diagnostics;
using iHentai.Core.Common.Controls;
using iHentai.Mvvm;
using Xamarin.Forms.Xaml;

namespace iHentai.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GalleryPage : MvvmPage
    {
        public GalleryPage()
        {
            InitializeComponent();
        }

        private void CollectionViewOnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            Debug.WriteLine($"VerticalChanged : {e.VerticalChanged}");
            if (e.VerticalChanged == 0d)
                return;
            SearchBar.IsVisible = e.VerticalChanged > 0d;
        }
    }
}