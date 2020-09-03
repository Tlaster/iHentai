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
using Windows.UI.Xaml.Navigation;
using iHentai.Data.Models;
using iHentai.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        SettingsViewModel ViewModel { get; } = new SettingsViewModel();

        public ElementTheme[] Themes { get; } =
        {
            ElementTheme.Default,
            ElementTheme.Light,
            ElementTheme.Dark,
        };

        public SettingsPage()
        {
            this.InitializeComponent();
        }

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
