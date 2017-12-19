using Windows.UI.Xaml.Controls;
using iHentai.Services;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Conet.Apis.Weibo.Views
{
    public sealed partial class LoginView : IContentView<LoginData>
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void TextBox_Paste(object sender, TextControlPasteEventArgs e)
        {
        }
    }
}