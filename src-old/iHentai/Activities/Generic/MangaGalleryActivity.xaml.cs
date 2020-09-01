using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using AngleSharp.Common;
using iHentai.Common;
using iHentai.Common.Tab;
using iHentai.Services.Core;
using iHentai.Services.Manhuagui.Model;
using iHentai.ViewModels.Generic;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Activities.Generic
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    partial class MangaGalleryActivity
    {
        private ImageEx _animationImageElement;
        public override ITabViewModel TabViewModel => ViewModel;
        public double ScrollVerticalOffset => 75 + TitleBarTop;
        public MangaGalleryViewModel ViewModel { get; private set; }
        public MangaGalleryActivity()
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
            ViewModel = new MangaGalleryViewModel(api);
        }

        private void AspectRatioView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OpenGallery(sender);
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            OpenGallery(sender);
        }

        private void OpenInNewTabClicked(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.Tag is IMangaGallery gallery)
            {
                StartNewTab<MangaDetailActivity>(gallery, Intent);
            }
        }

        private void OpenGallery(object sender)
        {
            if (sender is FrameworkElement element && element.Tag is IMangaGallery gallery)
            {
                _animationImageElement = element.FindDescendant<ImageEx>();
                StartActivity<MangaDetailActivity>(gallery, Intent);
            }
        }

        
        protected override void OnPrepareConnectedAnimation(ConnectedAnimationService service)
        {
            if (_animationImageElement == null)
            {
                return;
            }

            service.PrepareToAnimate("image", _animationImageElement)?.Also(it =>
            {
                it.Configuration = new DirectConnectedAnimationConfiguration();
            });
        }

        protected override void OnUsingConnectedAnimation(ConnectedAnimationService service)
        {
            service.GetAnimation("image")?.Also(it =>
            {
                it.TryStart(_animationImageElement);
                _animationImageElement = null;
            });
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var queryText = args.ChosenSuggestion?.ToString() ?? args.QueryText;
            ViewModel.Search(queryText);
        }
    }
}
