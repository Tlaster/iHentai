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
using iHentai.Data;
using iHentai.Data.Models;
using iHentai.Extensions;
using iHentai.Services.Models.Script;
using iHentai.ViewModels;
using iHentai.ViewModels.Local;
using iHentai.ViewModels.Script;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Newtonsoft.Json;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HistoryPage : Page
    {
        HistoryViewModel ViewModel { get; } = new HistoryViewModel();

        public HistoryPage()
        {
            this.InitializeComponent();
        }

        private async void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (!(e.ClickedItem is ReadingHistoryModel item))
            {
                return;
            }
            switch (item.ExtraInstance)
            {
                case ScriptGalleryHistoryExtra extra:
                    var api = await this.Resolve<IExtensionManager>().GetApiById(extra.ExtensionId);
                    if (api != null)
                    {
                        this.FindAscendant<RootView>().ContentFrame.Navigate(typeof(ReadingPage),
                            new ScriptGalleryReadingViewModel(api, JsonConvert.DeserializeObject<ScriptGalleryDetailModel>(extra.Detail)));
                    }
                    break;
                case ScriptGalleryChapterHistoryExtra extra:
                    break;
                case LocalGalleryHistoryExtra extra:
                    var gallery = LocalLibraryDb.Instance.FindGallery(extra.Path);
                    if (gallery != null)
                    {
                        this.FindAscendant<RootView>().ContentFrame
                            .Navigate(typeof(ReadingPage), new LocalReadingViewModel(gallery));
                    }
                    break;
            }
            
        }
    }
}
