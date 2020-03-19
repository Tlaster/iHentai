using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using AngleSharp.Common;
using iHentai.Common;
using iHentai.Common.Tab;
using iHentai.Services.EHentai;
using iHentai.Services.EHentai.Model;
using iHentai.ViewModels.EHentai;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Activities.EHentai
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    internal partial class GalleryDetailActivity
    {
        public GalleryDetailActivity()
        {
            InitializeComponent();
        }

        public override ITabViewModel TabViewModel => ViewModel;
        private GalleryDetailViewModel ViewModel { get; set; }

        protected internal override void OnCreate(object parameter)
        {
            base.OnCreate(parameter);
            var api = Intent.TryGet("api") as EHApi;
            switch (parameter)
            {
                case EHGallery gallery:
                    ViewModel = new GalleryDetailViewModel(gallery, api);
                    break;
                case string link:
                    ViewModel = new GalleryDetailViewModel(link, api);
                    break;
            }
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

        private void OpenInBrowser()
        {
            Launcher.LaunchUriAsync(new Uri(ViewModel.Gallery.Link));
        }

        private void OpenRead()
        {
            StartActivity<ReadingActivity>(new EHReadingViewModel(ViewModel.Api, ViewModel.Link));
        }

        private void TagClicked(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.Tag is EHGalleryTag tag)
            {
                StartActivity<GalleryActivity>(tag, Intent);
            }
        }

        private void GalleryImageTapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            if (sender is FrameworkElement element && element.Tag is IEHGalleryImage image)
            {
                StartActivity<ReadingActivity>(new EHReadingViewModel(ViewModel.Api, ViewModel.Link, image));
            }
        }

        private void UploaderClicked(object sender, RoutedEventArgs e)
        {
            StartActivity<GalleryActivity>("https://exhentai.org/uploader/" + ViewModel.Uploader,
                Intent.Also(it => it.Add("title", "uploader:" + ViewModel.Uploader)));
        }

        private void TagOpenInNewTabClicked(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.Tag is EHGalleryTag tag)
            {
                StartNewTab<GalleryActivity>(tag, Intent);
            }
        }

        private void TagWikiClicked(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.Tag is EHGalleryTag tag)
            {
                Launcher.LaunchUriAsync(new Uri("https://ehwiki.org/wiki/" + tag.Name));
            }
        }
    }
}