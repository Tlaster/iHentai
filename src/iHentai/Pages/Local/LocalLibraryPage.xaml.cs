using Windows.UI.Xaml.Controls;
using iHentai.Data.Models;
using iHentai.ViewModels.Local;
using Microsoft.Toolkit.Uwp.UI.Extensions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Pages.Local
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LocalLibraryPage : Page
    {
        internal LocalLibraryViewModel ViewModel { get; } = new LocalLibraryViewModel();
        public LocalLibraryPage()
        {
            this.InitializeComponent();
        }

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is LocalGalleryModel item)
            {
                this.FindAscendant<RootView>().ContentFrame.Navigate(typeof(ReadingPage), new LocalReadingViewModel(item));
            }
        }
    }
}
