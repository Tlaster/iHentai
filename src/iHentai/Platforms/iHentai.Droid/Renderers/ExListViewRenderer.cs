using System;
using Android.Widget;
using iHentai.Core.Common.Controls;
using iHentai.Droid.Common;
using iHentai.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ListView = Xamarin.Forms.ListView;

[assembly: ExportRenderer(typeof(ExListView), typeof(ExListViewRenderer))]

namespace iHentai.Droid.Renderers
{
    public class ExListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
                Control.Scroll -= Control_Scroll;
            else if (e.NewElement != null)
            {
                Control.Scroll += Control_Scroll;
            }
        }

        private void Control_Scroll(object sender, AbsListView.ScrollEventArgs e)
        {
            (Element as ExListView).InvokeScrollChanged(GetTopItemScrollY(), 0);
        }

        private int GetTopItemScrollY()
        {
            if (Control?.GetChildAt(0) == null) return 0;
            var topChild = Control.GetChildAt(0);
            return topChild.Top;
        }
    }
}