using iHentai.ViewModels;

namespace iHentai.Views
{
    public sealed partial class SettingsPage
    {
        //// TODO WTS: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/settings-codebehind.md
        //// TODO WTS: Change the URL for your privacy policy in the Resource File, currently set to https://YourPrivacyUrlGoesHere

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