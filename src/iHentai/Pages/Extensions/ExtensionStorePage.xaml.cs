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
using iHentai.Extensions.Models;
using iHentai.ViewModels.Extensions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Pages.Extensions
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExtensionStorePage : Page
    {
        private ExtensionsListViewModel ViewModel { get; } = ExtensionsListViewModel.Instance;

        public ExtensionStorePage()
        {
            this.InitializeComponent();
        }

        private async void OnInstallClicked(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is NetworkExtensionModel item)
            {
                try
                {
                    await ViewModel.InstallExtension(item);
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}
