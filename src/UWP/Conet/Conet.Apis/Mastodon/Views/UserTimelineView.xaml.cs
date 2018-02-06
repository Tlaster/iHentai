using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Conet.Apis.Core.ViewModels;
using Conet.Apis.Mastodon.ViewModels;
using iHentai.Services;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace Conet.Apis.Mastodon.Views
{
    public sealed partial class UserTimelineView : IContentView<UserTimelineViewModel>, INotifyPropertyChanged
    {
        public UserTimelineView()
        {
            this.InitializeComponent();
        }

        public UserTimelineViewModel ViewModel => DataContext as UserTimelineViewModel;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UserTimelineView_OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            OnPropertyChanged(nameof(ViewModel));
        }
    }
}
