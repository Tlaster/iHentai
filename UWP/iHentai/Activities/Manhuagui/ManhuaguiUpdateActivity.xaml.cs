using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using iHentai.Activities.EHentai;
using iHentai.Common;
using iHentai.Common.Tab;
using iHentai.Services.Manhuagui.Model;
using iHentai.ViewModels.Manhuagui;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Activities.Manhuagui
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    partial class ManhuaguiUpdateActivity
    {
        private ImageEx _animationImageElement;
        public override ITabViewModel TabViewModel => ViewModel;
        public ManhuaguiUpdateViewModel ViewModel { get; } = new ManhuaguiUpdateViewModel();
        public ManhuaguiUpdateActivity()
        {
            this.InitializeComponent();
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
            if (sender is FrameworkElement element && element.Tag is ManhuaguiGallery gallery)
            {
                StartNewTab<ManhuaguiDetailActivity>(gallery, Intent);
            }
        }

        private void OpenGallery(object sender)
        {
            if (sender is FrameworkElement element && element.Tag is ManhuaguiGallery gallery)
            {
                _animationImageElement = element.FindDescendant<ImageEx>();
                StartActivity<ManhuaguiDetailActivity>(gallery, Intent);
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
    }
}
