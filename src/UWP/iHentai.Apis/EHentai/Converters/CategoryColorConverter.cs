using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using iHentai.Apis.EHentai.Models;
using Microsoft.Toolkit.Uwp.Helpers;
using ColorHelper = Microsoft.Toolkit.Uwp.Helpers.ColorHelper;

namespace iHentai.Apis.EHentai.Converters
{
    public static class CategoryFlagsExtensions
    {
        public static Color ToColor(this CategoryFlags flags)
        {
            string color;
            switch (flags)
            {
                case CategoryFlags.Misc:
                    color = "#607D8B";
                    break;
                case CategoryFlags.Doujinshi:
                    color = "#F44336";
                    break;
                case CategoryFlags.Manga:
                    color = "#FFC107";
                    break;
                case CategoryFlags.ArtistCG:
                    color = "#FFEB3B";
                    break;
                case CategoryFlags.GameCG:
                    color = "#4CAF50";
                    break;
                case CategoryFlags.ImageSet:
                    color = "#3F51B5";
                    break;
                case CategoryFlags.Cosplay:
                    color = "#9C27B0";
                    break;
                case CategoryFlags.AsianPorn:
                    color = "#E91E63";
                    break;
                case CategoryFlags.Nonh:
                    color = "#2196F3";
                    break;
                case CategoryFlags.Western:
                    color = "#8BC34A";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return color.ToColor();
        }
    }

    public class CategoryColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var category = (CategoryFlags) value;
            return new SolidColorBrush(category.ToColor());
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}