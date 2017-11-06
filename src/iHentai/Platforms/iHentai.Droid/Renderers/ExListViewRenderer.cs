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

        private void Control_Scroll(object sender, AbsListView.ScrollEventArgs e)
        {
//            if (Control.ChildCount > 0)
//                (Element as ExListView).InvokeScrollChanged(GetScrollY(), 0);
        }

        private int GetScrollY()
        {
            var child = Control.GetChildAt(0); //this is the first visible row
            if (child == null) return 0;

            var scrollY = -child.Top;

            _listViewItemHeights.TryAdd(Control.FirstVisiblePosition, child.Height);

            for (var i = 0; i < Control.FirstVisiblePosition; ++i)
            {
                var hei = _listViewItemHeights[i];
                //Manual add hei each row into scrollY
                if (hei != null)
                    scrollY += hei;
            }

            return scrollY;
        }
    }
}