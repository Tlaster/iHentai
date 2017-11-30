﻿using System;
using System.Globalization;
using Windows.UI;
using Windows.UI.Xaml.Data;
using iHentai.Apis.EHentai.Models;

namespace iHentai.Apis.EHentai.Converters
{
    public class CategoryColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var category = (CategoryFlags) value;
            switch (category)
            {
                //case CategoryFlags.Misc:
                //    return Color.FromHex("607D8B");
                //case CategoryFlags.Doujinshi:
                //    return Color.FromHex("F44336");
                //case CategoryFlags.Manga:
                //    return Color.FromHex("FFC107");
                //case CategoryFlags.ArtistCG:
                //    return Color.FromHex("FFEB3B");
                //case CategoryFlags.GameCG:
                //    return Color.FromHex("4CAF50");
                //case CategoryFlags.ImageSet:
                //    return Color.FromHex("3F51B5");
                //case CategoryFlags.Cosplay:
                //    return Color.FromHex("9C27B0");
                //case CategoryFlags.AsianPorn:
                //    return Color.FromHex("E91E63");
                //case CategoryFlags.Nonh:
                //    return Color.FromHex("2196F3");
                //case CategoryFlags.Western:
                //    return Color.FromHex("8BC34A");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}