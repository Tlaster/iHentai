using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;
using iHentai.Common;
using iHentai.Extensions.Models;
using iHentai.Pages.Script;
using iHentai.ViewModels.Script;
using NavigationView = Microsoft.UI.Xaml.Controls.NavigationView;
using NavigationViewItem = Microsoft.UI.Xaml.Controls.NavigationViewItem;
using NavigationViewItemBase = Microsoft.UI.Xaml.Controls.NavigationViewItemBase;
using NavigationViewItemSeparator = Microsoft.UI.Xaml.Controls.NavigationViewItemSeparator;
using NavigationViewSelectionChangedEventArgs = Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238


namespace iHentai.Pages
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            RootNavigationView.IsTitleBarAutoPaddingEnabled = true;
            RootNavigationView.SelectedItem = Menus[0];
        }

        public ObservableCollection<NavigationViewItemBase> Menus { get; } =
        new ObservableCollection<NavigationViewItemBase>(new[] {
            new NavigationViewItem
            { Icon = new SymbolIcon(Symbol.Folder), Content = "本地库", Tag = "Local.LocalLibraryPage" },
            new NavigationViewItem
            { Icon = new SymbolIcon(Symbol.Globe), Content = "扩展源", SelectsOnInvoked = false }.Also(it =>
            {
                foreach (var (_, value) in HentaiApp.Instance.ExtensionManager.Extensions)
                {
                    it.MenuItems.Add(new NavigationViewItem { Content = value.Name, Tag = value });
                }
            }),
            new NavigationViewItem { Icon = new SymbolIcon(Symbol.Favorite), Content = "收藏" },
            new NavigationViewItem { Icon = new SymbolIcon(Symbol.Download), Content = "下载" },
        });

        private void NavigationView_SelectionChanged(NavigationView sender,
            NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                RootFrame.Navigate(typeof(SettingsPage));
            }
            else if (args.SelectedItem is FrameworkElement element)
            {
                switch (element.Tag)
                {
                    case string value:
                        RootFrame.Navigate(GetType().Assembly.GetType(GetType().Namespace + "." + value));
                        break;
                    case ExtensionManifest value:
                        RootFrame.Navigate(typeof(ScriptGalleryPage), new ScriptGalleryViewModel(value));
                        break;
                }
            }
        }
    }
}