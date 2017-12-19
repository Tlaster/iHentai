using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Conet.Apis.Core.Models.Interfaces;
using iHentai.Services;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Conet.Apis.Mastodon.Views
{
    [ContentType("Login")]
    public sealed partial class LoginView : IContentView<Apis>
    {
        public LoginView()
        {
            this.InitializeComponent();
        }
    }
}
