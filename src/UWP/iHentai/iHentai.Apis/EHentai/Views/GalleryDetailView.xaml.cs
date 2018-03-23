using System;
using System.Drawing;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using iHentai.Apis.EHentai.Models;
using iHentai.Services;
using Microsoft.Toolkit.Uwp.UI.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace iHentai.Apis.EHentai.Views
{
    public class ThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Vector2 vector2)
            {
                return new Thickness(vector2.X, vector2.Y, 0, 0);
            }
            return default(Thickness);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    [ContentKey("Detail")]
    public sealed partial class GalleryDetailView : IContentView<GalleryDetailModel>
    {
        public GalleryDetailView()
        {
            InitializeComponent();
        }

        private void Search_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ShowWiki_OnClick(object sender, RoutedEventArgs e)
        {
        }
    }
}