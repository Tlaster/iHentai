using Windows.UI.Xaml;
using Newtonsoft.Json.Linq;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace Conet.Apis.Mastodon.Views
{
    public sealed partial class UserView
    {
        public static readonly DependencyProperty AccountProperty = DependencyProperty.Register(
            nameof(Account), typeof(JObject), typeof(UserView), new PropertyMetadata(default(JObject)));

        public static readonly DependencyProperty RelationshipProperty = DependencyProperty.Register(
            nameof(Relationship), typeof(JObject), typeof(UserView), new PropertyMetadata(default(JObject)));

        public UserView()
        {
            InitializeComponent();
        }

        public JObject Account
        {
            get => (JObject) GetValue(AccountProperty);
            set => SetValue(AccountProperty, value);
        }

        public JObject Relationship
        {
            get => (JObject) GetValue(RelationshipProperty);
            set => SetValue(RelationshipProperty, value);
        }
    }
}