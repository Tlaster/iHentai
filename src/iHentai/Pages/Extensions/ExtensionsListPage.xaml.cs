using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using iHentai.Extensions;
using iHentai.Extensions.Models;
using iHentai.Pages.Script;
using iHentai.Services.Models.Script;
using iHentai.ViewModels.Extensions;
using iHentai.ViewModels.Script;

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

        private ExtensionsListViewModel ViewModel { get; } = ExtensionsListViewModel.Instance;

        private void BrowserNetworkExtensionClicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ExtensionStorePage));
        }

        private async void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is ExtensionManifest manifest)
            {
                var api = await this.Resolve<IExtensionManager>().GetApi(manifest);
                if (api != null)
                {
                    if (api.RequireLogin())
                    {
                        Frame.Navigate(typeof(ScriptLoginPage), new ScriptLoginViewModel(api));
                    }
                    else
                    {
                        Frame.Navigate(typeof(ScriptGalleryPage), new ScriptGalleryViewModel(api));
                    }
                    Frame.BackStack.Clear();
                }
            }
        }

        private async void OnUnInstallClicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is ExtensionManifest manifest)
            {
                try
                {
                    await ViewModel.UnInstallExtension(manifest);
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}