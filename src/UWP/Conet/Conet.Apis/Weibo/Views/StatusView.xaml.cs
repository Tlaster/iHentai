using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Conet.Apis.Weibo.Views.Converters;
using iHentai.Services;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats.MarkDown;
using Newtonsoft.Json.Linq;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace Conet.Apis.Weibo.Views
{
    [ContentKey(nameof(Weibo))]
    public sealed partial class StatusView : IContentView<JObject>
    {
        public StatusView()
        {
            this.InitializeComponent();
        }

        private void WeiboContentOnLinkClicked(object sender, LinkClickedEventArgs e)
        {
            Debug.WriteLine(e.Link);
            switch (Enum.Parse<WeiboLinkType>(e.Link.TrimStart('/').Split(':').FirstOrDefault()))
            {
                case WeiboLinkType.More:
                    break;
                case WeiboLinkType.Link:
                    break;
                case WeiboLinkType.User:
                    break;
                case WeiboLinkType.Topic:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void WeiboContentOnImageResolving(object sender, ImageResolvingEventArgs e)
        {
            e.Image = new BitmapImage(new Uri("https://assets-cdn.github.com/favicon.ico"));//TODO:
            e.Handled = true;
        }
    }
}
