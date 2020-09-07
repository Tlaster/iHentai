using System;
using System.Collections.Generic;
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
        private readonly List<ElementTheme> _themes = new List<ElementTheme>
        {
            ElementTheme.Default,
            ElementTheme.Light,
            ElementTheme.Dark
        };

        public SettingsPage()
        {
            InitializeComponent();
            ThemeRadio.SelectedIndex = _themes.IndexOf(ViewModel.Theme);
        }

        private SettingsViewModel ViewModel { get; } = new SettingsViewModel();

        private void OnRemoveLibraryClicked(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is LocalLibraryModel item)
            {
                ViewModel.RemoveLibraryFolder(item);
            }
        }

        private void OnThemeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.FirstOrDefault() is FrameworkElement element && element.Tag is string value)
            {
                if (Enum.TryParse<ElementTheme>(value, out var result))
                {
                    ViewModel.Theme = result;
                }
            }
        }
    }
}