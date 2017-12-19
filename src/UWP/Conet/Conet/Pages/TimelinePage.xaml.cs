using Conet.ViewModels;
using iHentai.Mvvm;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Conet.Pages
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
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