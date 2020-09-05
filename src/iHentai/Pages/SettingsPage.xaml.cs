using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using iHentai.Data.Models;
using iHentai.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Pages
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private SettingsViewModel ViewModel { get; } = new SettingsViewModel();

        public ElementTheme[] Themes { get; } =
        {
            ElementTheme.Default,
            ElementTheme.Light,
            ElementTheme.Dark
        };

        private void OnRemoveLibraryClicked(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is LocalLibraryModel item)
            {
                ViewModel.RemoveLibraryFolder(item);
            }
        }

        private void OnThemeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.FirstOrDefault() is ElementTheme theme)
            {
                ViewModel.Theme = theme;
            }
        }
    }
}