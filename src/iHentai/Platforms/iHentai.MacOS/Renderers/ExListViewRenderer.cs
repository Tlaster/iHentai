using AppKit;
using CoreGraphics;
using Foundation;
using iHentai.Core.Common.Controls;
using iHentai.MacOS.Renderers;
using ObjCRuntime;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

[assembly: ExportRenderer(typeof(ExListView), typeof(ExListViewRenderer))]

namespace iHentai.MacOS.Renderers
{
    public class ExListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                (Control as NSScrollView).ContentView.PostsBoundsChangedNotifications = true;
                NSNotificationCenter.DefaultCenter.AddObserver(this, new Selector("boundsDidChangeNotification"),
                    BoundsChangedNotification, (Control as NSScrollView).ContentView);
            }
            if (e.OldElement != null)
                NSNotificationCenter.DefaultCenter.RemoveObserver(this, "boundsDidChangeNotification",
                    (Control as NSScrollView).ContentView);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                NSNotificationCenter.DefaultCenter.RemoveObserver(this, "boundsDidChangeNotification",
                    (Control as NSScrollView).ContentView);
            base.Dispose(disposing);
        }

        [Export("boundsDidChangeNotification")]
        public void BoundsDidChangeNotification()
        {
            (Element as ExListView).InvokeScrollChanged((Control as NSScrollView).ContentView.Bounds.Location.Y,
                (Control as NSScrollView).ContentView.Bounds.Location.X);
        }

        public override void ScrollPoint(CGPoint aPoint)
        {
            base.ScrollPoint(aPoint);
        }

        public override void ScrollWheel(NSEvent theEvent)
        {
            base.ScrollWheel(theEvent);
        }
    }
}