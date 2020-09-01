using Windows.UI.Xaml.Controls;
using iHentai.Pages;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace iHentai
{
    public sealed partial class RootView
    {
        internal Frame ContentFrame => RootFrame;
        public RootView()
        {
            this.InitializeComponent();
            RootFrame.SourcePageType = typeof(HomePage);
        }
    }
}
