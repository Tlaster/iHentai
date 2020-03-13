using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
using iHentai.Common;
using iHentai.Common.Tab;
using iHentai.Services.EHentai.Model;
using iHentai.ViewModels;
using iHentai.ViewModels.EHentai;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Activities.EHentai
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    partial class GalleryActivity
    {
        private FrameworkElement _animationImageElement;
        public override ITabViewModel TabViewModel => ViewModel;

        GalleryViewModel ViewModel { get; } = new GalleryViewModel();

        public GalleryActivity()
        {
            this.InitializeComponent();
        }

        private async void AspectRatioView_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.Tag is EHGallery gallery)
            {
                e.Handled = true;
                _animationImageElement = element.FindDescendant<ImageEx>();
                StartActivity<GalleryDetailActivity>(gallery);
            }
        }

        protected override void OnPrepareConnectedAnimation(ConnectedAnimationService service)
        {
            service.PrepareToAnimate("image", _animationImageElement).Configuration = new DirectConnectedAnimationConfiguration();
        }

        protected override void OnUsingConnectedAnimation(ConnectedAnimationService service)
        {
            service.GetAnimation("image")?.Also(it =>
            {
                it.TryStart(_animationImageElement);
                _animationImageElement = null;
            });
        }
    }
}
