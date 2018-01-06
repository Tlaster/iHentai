using iHentai.Mvvm;
using iHentai.ViewModels;

namespace iHentai.Views
{
    public sealed partial class SettingsPage : IMvvmView<SettingsViewModel>
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        public new SettingsViewModel ViewModel
        {
            get => (SettingsViewModel) base.ViewModel;
            set => base.ViewModel = value;
        }
    }
}