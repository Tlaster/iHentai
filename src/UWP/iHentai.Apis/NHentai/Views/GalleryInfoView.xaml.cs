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
using Humanizer;
using iHentai.Apis.Core.Models.Interfaces;
using iHentai.Apis.NHentai.Models;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace iHentai.Apis.NHentai.Views
{
    public sealed partial class GalleryInfoView : IGalleryInfoView<GalleryModel>
    {
        public GalleryInfoView()
        {
            this.InitializeComponent();
        }

        private void GalleryInfoView_OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            LanguageTextBlock.Text = string.Empty;
            if (!(args.NewValue is GalleryModel))
                return;
            var model = (GalleryModel) args.NewValue;
            if (model.Tags == null)
                return;
            var tags = model.Tags?.GroupBy(item => item.Type).ToDictionary(item => item.Key, item => item.ToList());
            if (tags.TryGetValue("language", out var res))
            {
                LanguageTextBlock.Text = res.LastOrDefault()?.Name?.Humanize(LetterCasing.Title) ?? string.Empty;
            }
        }
    }
}
