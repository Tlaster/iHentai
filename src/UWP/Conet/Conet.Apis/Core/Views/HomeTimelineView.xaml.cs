using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Conet.Apis.Core.ViewModels;
using iHentai.Services;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace Conet.Apis.Core.Views
{
    public sealed partial class HomeTimelineView : IContentView<HomeTimelineViewModel>, INotifyPropertyChanged
    {
        public HomeTimelineView()
        {
            this.InitializeComponent();
        }

        public HomeTimelineViewModel ViewModel => (HomeTimelineViewModel) DataContext;

        private void HomeTimelineView_OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            OnPropertyChanged(nameof(ViewModel));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
