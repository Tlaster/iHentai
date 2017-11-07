using System;
using System.Diagnostics;
using iHentai.Core.Common.Controls;
using iHentai.Mvvm;
using Xamarin.Forms;
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
            var res = SearchBar.TranslationY + e.VerticalChanged;
            if (e.VerticalChanged > 0)
                SearchBar.TranslationY = Math.Min(0, res);
            else if (e.VerticalChanged < 0)
                SearchBar.TranslationY =
                    Math.Max(
                        -(SearchBar.Height + SearchBar.Padding.VerticalThickness * 2 +
                          SearchBar.Margin.VerticalThickness * 2), res);
            //SearchBar.IsVisible = e.VerticalChanged > 0d;
        }
    }
}