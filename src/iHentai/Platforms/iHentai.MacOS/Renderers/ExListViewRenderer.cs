using AppKit;
using CoreGraphics;
using ObjCRuntime;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

namespace iHentai.MacOS.Renderers
{
    public class ExListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);
        }

        public override void ScrollWheel(NSEvent theEvent)
        {
            base.ScrollWheel(theEvent);
        }
    }
}