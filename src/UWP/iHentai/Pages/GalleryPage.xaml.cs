using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using FFImageLoading;
using iHentai.Apis.Core.Models.Interfaces;
using iHentai.ViewModels;
using iHentai.Views;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using WaterFallView;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Pages
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GalleryPage
    {
        private Image _tappedItem;

        public GalleryPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
        }

        public new GalleryViewModel ViewModel
        {
            get => (GalleryViewModel) base.ViewModel;
            set => base.ViewModel = value;
        }

        protected override void SaveState(Dictionary<string, object> bundleState)
        {
            base.SaveState(bundleState);
            bundleState.Add("tappedItem", _tappedItem);
        }

        protected override void RestoreState(Dictionary<string, object> bundleState)
        {
            base.RestoreState(bundleState);
            if (bundleState.TryGetValue("tappedItem", out var item))
            {
                _tappedItem = (Image) item;
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (MoreInfoCanvas.Visibility == Visibility.Visible)
            {
                ConnectedAnimationService.GetForCurrentView().GetAnimation("detail_image")?.TryStart(MoreInfoImage);
            }
            else if (_tappedItem != null)
            {
                ConnectedAnimationService.GetForCurrentView().GetAnimation("detail_image")?.TryStart(_tappedItem);
                _tappedItem = null;
                //var container = WaterfallLayout.GetContainerFormItem(e.Parameter);
                //if (container != null)
                //{
                //    ConnectedAnimationService.GetForCurrentView().GetAnimation("detail_image")?.TryStart(container.FindDescendant<Image>());
                //}
            }
        }

        private void HideMoreInfo()
        {
            if (MoreInfoImage != null && MoreInfoContent.DataContext != null)
                ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("image", MoreInfoImage);

            MoreInfoCanvas.Visibility = Visibility.Collapsed;

            if (MoreInfoImage != null && MoreInfoContent.DataContext != null && MoreInfoContent.Tag != null)
            {
                var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("image");
                animation.TryStart(MoreInfoContent.Tag as Image);
            }
            MoreInfoContent.Tag = null;
            MoreInfoContent.DataContext = null;
        }

        private void GalleryGridItem_OnMoreInfoRequest(object sender, IGalleryModel e)
        {
            var galleryGridItem = (GalleryGridItem) sender;
            var ffimage = galleryGridItem.FindDescendant<Image>();
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

            MoreInfoCanvas.Visibility = Visibility.Visible;
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("image", ffimage).TryStart(MoreInfoImage);
        }

        private void MoreInfoCanvas_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            HideMoreInfo();
        }

        private void GalleryPage_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.PreviousSize.Width == 0d && e.PreviousSize.Height == 0d)
            {
                return;
            }
            HideMoreInfo();
        }

        //private void WaterfallLayout_OnItemTapped(object sender, ItemTappedEventArgs e)
        //{
        //    ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("detail_image", e.Container.FindDescendant<Image>());
        //    ViewModel.GoDetail(e.Item as IGalleryModel);
        //}

        private void MoreInfoImage_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("detail_image", MoreInfoImage);
            ViewModel.GoDetail(MoreInfoImage.DataContext as IGalleryModel);
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            _tappedItem = (sender as GalleryGridItem).FindDescendant<Image>();
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("detail_image", _tappedItem);

            ViewModel.GoDetail((sender as GalleryGridItem).DataContext as IGalleryModel);
        }
    }
}