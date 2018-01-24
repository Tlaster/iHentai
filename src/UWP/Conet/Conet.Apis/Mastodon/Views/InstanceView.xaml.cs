using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using iHentai.Services;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace Conet.Apis.Mastodon.Views
{
    public sealed partial class InstanceView : IContentView<InstanceData>
    {
        private InstanceData _cache;

        public InstanceView()
        {
            InitializeComponent();
        }

        private void InstanceView_OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (!(args.NewValue is InstanceData data) || _cache == data) return;
            _cache = data;
            nameof(Mastodon).Get<Apis>().User(data, data.Uid).ContinueWith(async t =>
            {
                if (t.Result == null || t.Exception != null) return;
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        UserImage.Source = t.Result.Value<string>("avatar");
                        ProgressRing.IsActive = false;
                    });
            }).ConfigureAwait(false);
        }
    }
}