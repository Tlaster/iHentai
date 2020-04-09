using System;
using System.Linq;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
using AngleSharp.Common;
using iHentai.Common;
using iHentai.Common.Tab;
using iHentai.Services.Core;
using iHentai.Services.Manhuagui;
using iHentai.Services.Manhuagui.Model;
using iHentai.ViewModels.Manhuagui;
using Microsoft.Toolkit.Helpers;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Activities.Generic
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    partial class MangaDetailActivity
    {
        public override ITabViewModel TabViewModel => ViewModel;
        public ManhuaguiDetailViewModel ViewModel { get; private set; }

        public MangaDetailActivity()
        {
            this.InitializeComponent();
        }

        protected internal override void OnCreate(object parameter)
        {
            base.OnCreate(parameter);
            if (!Intent.ContainsKey("api"))
            {
                Finish();
            }
            var api = Intent.TryGet("api") as IMangaApi;
            switch (parameter)
            {
                case ManhuaguiGallery gallery:
                    ViewModel = new ManhuaguiDetailViewModel(api, gallery);
                    break;
                    break;
            }
        }
        private void OpenInBrowser()
        {
            Launcher.LaunchUriAsync(new Uri(Singleton<ManhuaguiApi>.Instance.Host + ViewModel.Gallery.Link));
        }

        private void OpenRead()
        {
            if (ViewModel.Detail == null)
            {
                return;
            }
            StartActivity<ReadingActivity>(new ManhuaguiReadingViewModel(ViewModel.Detail.Chapters?.LastOrDefault()?.Link, ViewModel.Detail));
        }
        protected override void OnUsingConnectedAnimation(ConnectedAnimationService service)
        {
            service.GetAnimation("image")?.TryStart(DetailImage);
        }

        protected internal override bool OnBackRequest()
        {
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("image", DetailImage)?.Also(it =>
            {
                it.Configuration = new DirectConnectedAnimationConfiguration();
            });
            return base.OnBackRequest();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.Tag is ManhuaguGalleryChapter chapter)
            {
                StartActivity<ReadingActivity>(new ManhuaguiReadingViewModel(chapter.Link, ViewModel.Detail));
            }
        }
    }
}
