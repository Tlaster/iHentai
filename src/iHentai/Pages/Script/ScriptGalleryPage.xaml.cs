using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using iHentai.Services;
using iHentai.Services.Models.Script;
using iHentai.ViewModels.Script;
using Microsoft.Toolkit.Uwp.UI.Animations;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Pages.Script
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ScriptGalleryPage : Page
    {
        public ScriptGalleryPage()
        {
            InitializeComponent();
        }

        private ScriptGalleryViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is ScriptGalleryViewModel viewModel)
            {
                ViewModel = viewModel;
            }
        }

        private async void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is ScriptGalleryModel item && ViewModel.Api is ScriptApi api)
            {
                if (await api.HasDetail())
                {    
                    Frame.SetListDataItemForNextConnectedAnimation(item);
                    Frame.Navigate(typeof(ScriptGalleryDetailPage), new ScriptGalleryDetailViewModel(api, item));
                }
                else if (await api.HasGalleryImages())
                {
                }
            }
        }

        private void AutoSuggestBox_OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ViewModel.Search(args.QueryText);
        }

        private void AutoSuggestBox_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (sender.Text.Length == 0)
            {
                ViewModel.Reset();
            }
        }
    }
}