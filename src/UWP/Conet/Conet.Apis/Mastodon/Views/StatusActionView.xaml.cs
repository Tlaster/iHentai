using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json.Linq;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace Conet.Apis.Mastodon.Views
{
    public sealed partial class StatusActionView : UserControl
    {
        public static readonly DependencyProperty ActionTypeProperty = DependencyProperty.Register(
            nameof(ActionType), typeof(string), typeof(StatusActionView), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ActionAccountProperty = DependencyProperty.Register(
            nameof(ActionAccount), typeof(JObject), typeof(StatusActionView), new PropertyMetadata(default(JObject)));

        public StatusActionView()
        {
            InitializeComponent();
        }

        public JObject ActionAccount
        {
            get => (JObject) GetValue(ActionAccountProperty);
            set => SetValue(ActionAccountProperty, value);
        }

        public string ActionType
        {
            get => (string) GetValue(ActionTypeProperty);
            set => SetValue(ActionTypeProperty, value);
        }
    }
}