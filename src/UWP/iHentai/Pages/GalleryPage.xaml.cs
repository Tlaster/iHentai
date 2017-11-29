using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using FFImageLoading;
using iHentai.Apis.Core.Models.Interfaces;
using iHentai.ViewModels;
using iHentai.Views;
using Marduk.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Pages
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GalleryPage
    {
        public GalleryPage()
        {
            InitializeComponent();
        }

        public new GalleryViewModel ViewModel
        {
            get => (GalleryViewModel) base.ViewModel;
            set => base.ViewModel = value;
        }


        private void HideMoreInfo()
        {
            if (MoreInfoImage != null && MoreInfoContent.DataContext != null)
                ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("image", MoreInfoImage);

            MoreInfoCanvas.Visibility = Visibility.Collapsed;

            if (MoreInfoImage != null && MoreInfoContent.DataContext != null)
            {
                var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("image");
                animation.TryStart(MoreInfoContent.Tag as FFImage);
            }
            MoreInfoContent.Tag = null;
            MoreInfoContent.DataContext = null;
        }

        private void GalleryGridItem_OnMoreInfoRequest(object sender, IGalleryModel e)
        {
            var galleryGridItem = (GalleryGridItem) sender;
            var ffimage = galleryGridItem.FindDescendant<FFImage>();
            var container = galleryGridItem.FindAscendant<VirtualizingViewItem>();
            if (container == null || ffimage == null)
                return;
            MoreInfoContent.Tag = ffimage;
            MoreInfoContent.DataContext = e;
            MoreInfoImage.Width = ffimage.ActualWidth * 1.2;
            MoreInfoImage.Height = ffimage.ActualHeight * 1.2;
            MoreInfoTitle.MaxWidth = MoreInfoImage.Width;
            MoreInfoContent.Measure(new Size(double.MaxValue, double.MaxValue));

            var point = container.TransformToVisual(this).TransformPoint(new Point(0, 0));

            var x = point.X - (MoreInfoContent.DesiredSize.Width - container.ActualWidth) / 2;
            var y = point.Y - (MoreInfoContent.DesiredSize.Height - container.ActualHeight) / 2;

            x = Math.Max(x, 10);
            x = Math.Min(x, ActualWidth - MoreInfoContent.DesiredSize.Width - 10);

            y = Math.Max(y, 10);
            y = Math.Min(y, ActualHeight - MoreInfoContent.DesiredSize.Height - 10);

            Canvas.SetLeft(MoreInfoContent, x);
            Canvas.SetTop(MoreInfoContent, y);

            var centerX = point.X + container.ActualWidth / 2 - x;
            var centerY = point.Y + container.ActualHeight / 2 - y;

            VisualEx.SetCenterPoint(MoreInfoContent, new Vector3((float) centerX, (float) centerY, 0).ToString());

            //ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("image", ffimage).TryStart(MoreInfoImage);
            MoreInfoCanvas.Visibility = Visibility.Visible;
        }

        private void MoreInfoCanvas_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            HideMoreInfo();
        }

        private void GalleryPage_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            HideMoreInfo();
        }
    }
}