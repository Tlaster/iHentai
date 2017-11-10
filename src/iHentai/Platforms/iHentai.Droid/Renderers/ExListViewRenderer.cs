using System.Collections.Generic;
using Android.Widget;
using iHentai.Core.Common.Controls;
using iHentai.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ListView = Xamarin.Forms.ListView;

[assembly: ExportRenderer(typeof(ExListView), typeof(ExListViewRenderer))]

namespace iHentai.Droid.Renderers
{
    public class ExListViewRenderer : ListViewRenderer
    {
        private readonly Dictionary<int, int> _listViewItemHeights = new Dictionary<int, int>();

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
                Control.Scroll -= Control_Scroll;
            else if (e.NewElement != null)
                Control.Scroll += Control_Scroll;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && Control != null)
                Control.Scroll -= Control_Scroll;
            base.Dispose(disposing);
        }

        private void Control_Scroll(object sender, AbsListView.ScrollEventArgs e)
        {
            if (Control.ChildCount > 0)
                (Element as ExListView).InvokeScrollChanged(GetScrollY(), 0);
        }

        private int GetScrollY()
        {
            var child = Control.GetChildAt(0);
            if (child == null) return 0;

            var scrollY = -child.Top;

            _listViewItemHeights.TryAdd(Control.FirstVisiblePosition, child.Height);

            for (var i = 0; i < Control.FirstVisiblePosition; ++i)
                if (_listViewItemHeights.TryGetValue(i, out var hei))
                    scrollY += hei;

            return scrollY;
        }
    }
}