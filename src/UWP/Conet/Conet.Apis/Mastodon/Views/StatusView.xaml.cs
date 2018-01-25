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
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Conet.Apis.Mastodon.Model;
using iHentai.Basic.Extensions;
using iHentai.Services;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Newtonsoft.Json.Linq;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace Conet.Apis.Mastodon.Views
{
    class StatusDataContextConverter : IValueConverter
    {
        public string ReblogPath { get; set; } = "reblog";

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is JObject jObject))
            {
                return null;
            }

            return jObject[ReblogPath].HasValues ? jObject[ReblogPath] : jObject;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    [ContentKey(nameof(Mastodon))]
    public sealed partial class StatusView : IContentView<JObject>
    {
        public StatusView()
        {
            this.InitializeComponent();
            //Loaded += OnLoaded;
        }

        public ListView ParentListView => this.FindAscendant<ListView>();

        //private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        //{
        //    Loaded -= OnLoaded;
        //    ParallaxView.Source = this.FindAscendant<ListView>();
        //}

        //private void StatusView_OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        //{
        //    if (!(args.NewValue is JObject jObject))
        //    {
        //        return;
        //    }

        //    var a = jObject["reblog"];
        //    Root.DataContext = jObject["reblog"] ?? jObject;
        //}
    }
}
