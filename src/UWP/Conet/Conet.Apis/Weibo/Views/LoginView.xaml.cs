using System;
using System.Text;
using Windows.UI.Xaml.Controls;
using iHentai.Services;
using static System.Convert;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Conet.Apis.Weibo.Views
{
    public sealed partial class LoginView : IContentView<LoginData>
    {
        private const string START = "SS", SEPERATOR = "::", END = "EE";
        public LoginView()
        {
            InitializeComponent();
        }

        private async void TextBox_Paste(object sender, TextControlPasteEventArgs e)
        {
            var dataPackageView = Windows.ApplicationModel.DataTransfer.Clipboard.GetContent();
            if (dataPackageView.Contains(Windows.ApplicationModel.DataTransfer.StandardDataFormats.Text))
            {
                try
                {
                    //SSMjExMTYwNjc5OjoxZTZlMzNkYjA4ZjkxOTIzMDZjNGFmYTBhNjFhZDU2Yzo6aHR0cDovL29hdXRoLndlaWNvLmNjOjplbWFpbCxkaXJlY3RfbWVzc2FnZXNfcmVhZCxkaXJlY3RfbWVzc2FnZXNfd3JpdGUsZnJpZW5kc2hpcHNfZ3JvdXBzX3JlYWQsZnJpZW5kc2hpcHNfZ3JvdXBzX3dyaXRlLHN0YXR1c2VzX3RvX21lX3JlYWQsZm9sbG93X2FwcF9vZmZpY2lhbF9taWNyb2Jsb2csaW52aXRhdGlvbl93cml0ZTo6Y29tLmVpY28ud2VpY286OkVFEE
                    var text = await dataPackageView.GetTextAsync();
                    var data = Decode(text.Trim());
                    AppID_TB.Text = data[0];
                    AppSecret_TB.Text = data[1];
                    RedirectUri_TB.Text = data[2];
                    Scope_TB.Text = data[3];
                    PackageName_TB.Text = data[4];
                    e.Handled = true;
                }
                catch
                {
                }
            }
        }
        private static bool CheckData(string data)
            => data.StartsWith(START) && data.Length > START.Length + END.Length && data.EndsWith(END);

        public static string[] Decode(string data)
        {
            if (!CheckData(data))
                throw new ArgumentException($"{data} is invalid");
            data = data.Substring(START.Length, data.Length - END.Length - 2);
            return Encoding.UTF8.GetString(FromBase64String(data)).Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
        }

    }
}