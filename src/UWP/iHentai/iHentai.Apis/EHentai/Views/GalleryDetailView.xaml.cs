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
using iHentai.Apis.Core.Models.Interfaces;
using iHentai.Apis.EHentai.Models;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace iHentai.Apis.EHentai.Views
{
    [ContentType(ContentTypes.Detail)]
    public sealed partial class GalleryDetailView : IGalleryContentView<GalleryDetailModel>
    {
        public GalleryDetailView()
        {
            this.InitializeComponent();
        }

        private void Search_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void ShowWiki_OnClick(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
