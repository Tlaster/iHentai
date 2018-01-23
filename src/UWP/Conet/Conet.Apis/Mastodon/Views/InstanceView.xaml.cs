using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Conet.Apis.Mastodon.Model;
using iHentai.Services;
using Newtonsoft.Json.Linq;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace Conet.Apis.Mastodon.Views
{
    public sealed partial class InstanceView : IContentView<InstanceData>
    {
        public InstanceView()
        {
            this.InitializeComponent();
        }

        private void InstanceView_OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (!(args.NewValue is InstanceData data))
            {
                return;
            }

            nameof(Mastodon).Get<Apis>().User(data, data.Uid).ContinueWith(async t =>
            {
                if (t.Result == null || t.Exception != null) return;
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        UserImage.Source = (t.Result as AccountModel).Avatar;
                        ProgressRing.IsActive = false;
                    });
            }).ConfigureAwait(false);
        }
    }
}
