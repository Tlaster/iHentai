using Conet.Mastodon.ViewModels;
using iHentai.Mvvm;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Conet.Mastodon.Views
{
    /// <summary>
    ///     可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class TimelinePage : IMvvmView<TimelineViewModel>
    {
        public TimelinePage()
        {
            InitializeComponent();
        }

        public new TimelineViewModel ViewModel
        {
            get => (TimelineViewModel) base.ViewModel;
            set => base.ViewModel = value;
        }
    }
}