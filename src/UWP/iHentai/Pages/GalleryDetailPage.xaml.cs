using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using iHentai.Mvvm;
using iHentai.Paging;
using iHentai.ViewModels;
using iHentai.Views;
using Microsoft.Toolkit.Uwp.UI.Extensions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Pages
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GalleryDetailPage : IMvvmView<GalleryDetailViewModel>
    {
        public GalleryDetailPage()
        {
            InitializeComponent();
        }

        public new GalleryDetailViewModel ViewModel
        {
            get => (GalleryDetailViewModel) base.ViewModel;
            set => base.ViewModel = value;
        }

        protected override void OnStart()
        {
            base.OnStart();
            ConnectedAnimationService.GetForCurrentView().GetAnimation("detail_image")?.TryStart(ThumbImage);
        }

        protected override void OnClose()
        {
            base.OnClose();
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("detail_image", ThumbImage);
        }
        
    }
}