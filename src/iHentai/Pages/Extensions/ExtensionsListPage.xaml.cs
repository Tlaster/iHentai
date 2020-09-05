using Windows.UI.Xaml.Controls;
using iHentai.ViewModels.Extensions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Pages.Extensions
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExtensionsListPage : Page
    {
        public ExtensionsListPage()
        {
            InitializeComponent();
        }

        private ExtensionsListViewModel ViewModel { get; } = new ExtensionsListViewModel();

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
        }
    }
}