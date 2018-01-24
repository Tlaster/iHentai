using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using iHentai.Services;
using Newtonsoft.Json.Linq;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace Conet.Apis.Weibo.Views
{
    public sealed partial class InstanceView : IContentView<InstanceData>
    {
        public InstanceView()
        {
            InitializeComponent();
        }


        private void InstanceView_OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (!(args.NewValue is InstanceData data)) return;
            nameof(Weibo).Get<Apis>().User(data, data.Uid.ToString()).ContinueWith(async t =>
            {
                if (t.Result == null || t.Exception != null) return;
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        UserImage.Source = (t.Result as JToken).Value<string>("avatar_large");
                        ProgressRing.IsActive = false;
                    });
            }).ConfigureAwait(false);
        }
    }
}