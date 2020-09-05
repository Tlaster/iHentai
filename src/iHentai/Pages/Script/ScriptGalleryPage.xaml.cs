using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using iHentai.Services;
using iHentai.Services.Models.Script;
using iHentai.ViewModels.Script;

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

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is ScriptGalleryModel item && ViewModel.Api is ScriptApi api)
            {
                if (api.HasDetail())
                {
                    Frame.Navigate(typeof(ScriptGalleryDetailPage), new ScriptGalleryDetailViewModel(api, item));
                }
                else if (api.HasGalleryImages())
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