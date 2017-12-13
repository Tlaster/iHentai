using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Data;
using iHentai.Apis.NHentai.Models;

namespace iHentai.Apis.NHentai.Converters
{
    public class ThumbListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is GalleryModel model))
            {
                return null;
            }
            return Enumerable.Range(0, model.Images.Pages.Length).Select(item => new
            {
                Link = $"https://t.nhentai.net/galleries/{model.MediaId}/{item}t.{(string.Equals(Images.Cover.Type, "j", StringComparison.OrdinalIgnoreCase) ? "jpg" : "png")}",
                Page = item
            });
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
