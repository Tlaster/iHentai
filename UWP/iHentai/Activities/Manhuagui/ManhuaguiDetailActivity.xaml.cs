using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using iHentai.Common;
using iHentai.Common.Tab;
using iHentai.Services.Manhuagui;
using iHentai.Services.Manhuagui.Model;
using iHentai.ViewModels.EHentai;
using iHentai.ViewModels.Manhuagui;
using Microsoft.Toolkit.Helpers;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Activities.Manhuagui
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    partial class ManhuaguiDetailActivity
    {
        public override ITabViewModel TabViewModel => ViewModel;
        public ManhuaguiDetailViewModel ViewModel { get; private set; }

        public ManhuaguiDetailActivity()
        {
            this.InitializeComponent();
        }

        protected internal override void OnCreate(object parameter)
        {
            base.OnCreate(parameter);
            switch (parameter)
            {
                case ManhuaguiGallery gallery:
                    ViewModel = new ManhuaguiDetailViewModel(gallery);
                    break;
                case string link:
                    ViewModel = new ManhuaguiDetailViewModel(link);
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
