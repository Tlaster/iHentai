using System;
using System.Collections.Generic;
using System.Text;
using iHentai.Core.Common.Controls;
using iHentai.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExListView), typeof(ExListViewRenderer))]
namespace iHentai.iOS.Renderers
{
    public class ExListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);
            //if (e.OldElement != null)
            //{
            //    Control.Scrolled -= Control_Scrolled;
            //}
            //else if (e.NewElement != null)
            //{
            //    Control.Scrolled += Control_Scrolled;
            //}
            
        }



        private void Control_Scrolled(object sender, EventArgs e)
        {
            (Element as ExListView).InvokeScrollChanged(Control.ContentOffset.Y, Control.ContentOffset.X);
        }
    }
}