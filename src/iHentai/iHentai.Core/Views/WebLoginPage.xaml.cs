using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHentai.Core.Common.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iHentai.Core.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WebLoginPage 
	{
		public WebLoginPage ()
		{
			InitializeComponent ();
		}

	    private void ExWebView_OnNavigated(object sender, CookieNavigatedEventArgs e)
	    {
	        
	    }
	}
}