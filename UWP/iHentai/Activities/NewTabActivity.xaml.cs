using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using iHentai.Activities.EHentai;
using iHentai.Common;
using iHentai.Common.Tab;
using iHentai.Services.EHentai;
using iHentai.ViewModels;
using Microsoft.Toolkit.Helpers;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Activities
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    partial class NewTabActivity
    {
        public override ITabViewModel TabViewModel => ViewModel;

        public NewTabViewModel ViewModel { get; } = new NewTabViewModel();

        public NewTabActivity()
        {
            this.InitializeComponent();
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is KeyValuePair<string, Type> item)
            {
                //TODO: a better way
                if (item.Key == "exHentai")
                {
                    if (Singleton<ExApi>.Instance.RequireLogin)
                    {
                        StartActivity<LoginActivity>();
                    }
                    else
                    {
                        StartActivity<GalleryActivity>(intent: new Dictionary<string, object>
                        {
                            { "api", Singleton<ExApi>.Instance}
                        });
                    }
                }
                else
                {
                    StartActivity(item.Value);
                }
                Finish();
            }
        }
    }
}
