using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using AngleSharp.Common;
using iHentai.Common;
using iHentai.Common.Tab;
using iHentai.Services.EHentai;
using iHentai.Services.EHentai.Model;
using iHentai.ViewModels;
using iHentai.ViewModels.EHentai;
using Microsoft.Toolkit.Helpers;
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

        GalleryViewModel ViewModel { get; set; }

        public GalleryActivity()
        {
            this.InitializeComponent();
        }

        protected internal override void OnCreate(object parameter)
        {
            base.OnCreate(parameter);
            if (!Intent.ContainsKey("api"))
            {
                Intent.Add("api", Singleton<EHApi>.Instance);
            }
            var api = Intent.TryGet("api") as EHApi;
            ViewModel = new GalleryViewModel(api);

        }

        private async void AspectRatioView_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.Tag is EHGallery gallery)
            {
                e.Handled = true;
                _animationImageElement = element.FindDescendant<ImageEx>();
                StartActivity<GalleryDetailActivity>(gallery, Intent);
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

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                sender.ItemsSource = ViewModel.GetSearchSuggestion(sender.Text);
            }
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var queryText = args.ChosenSuggestion?.ToString() ?? args.QueryText;
            ViewModel.Search(queryText ?? string.Empty);
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            sender.Text = args.SelectedItem?.ToString() ?? string.Empty;
        }
    }
}
