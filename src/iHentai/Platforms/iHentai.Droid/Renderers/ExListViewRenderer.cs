using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using iHentai.Core.Common.Controls;
using iHentai.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExListView), typeof(ExListViewRenderer))]
namespace iHentai.Droid.Renderers
{
    public class ExListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                Control.Scroll -= Control_Scroll;
            }
            else if (e.NewElement != null)
            {
                Control.Scroll += Control_Scroll;
            }
        }

        private void Control_Scroll(object sender, AbsListView.ScrollEventArgs e)
        {

            
        }
        
    }
}