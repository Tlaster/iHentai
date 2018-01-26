using System;
using Windows.UI.Xaml.Data;
using iHentai.Services;
using Newtonsoft.Json.Linq;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace Conet.Apis.Mastodon.Views
{
    internal class StatusDataContextConverter : IValueConverter
    {
        public string ReblogPath { get; set; } = "reblog";

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is JObject jObject)) return null;

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
            InitializeComponent();
        }
    }
}