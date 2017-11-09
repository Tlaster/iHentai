using System;
using Foundation;
using iHentai.Core.Common.Controls;
using iHentai.iOS.Renderers;
using UIKit;
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
            if (e.NewElement != null)
            {
                Control.Delegate = new ListViewDelegate(this);
            }
            if (e.OldElement != null)
            {
                Control.Delegate = null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Control.Delegate = null;   
            }
            base.Dispose(disposing);
        }

        //private void Control_Scrolled(object sender, EventArgs e)
        //{
        //    (Element as ExListView).InvokeScrollChanged(Control.ContentOffset.Y, Control.ContentOffset.X);
        //}
    }

    internal class ListViewDelegate : UITableViewDelegate
    {
        private readonly ListView _element;
        private readonly UITableViewSource _source;

        public ListViewDelegate(ListViewRenderer renderer)
        {
            _element = renderer.Element;
            _source = renderer.Control.Source;
        }
        
        public override void DraggingEnded(UIScrollView scrollView, bool willDecelerate)
        {
            _source.DraggingEnded(scrollView, willDecelerate);
        }

        public override void DraggingStarted(UIScrollView scrollView)
        {
            _source.DraggingStarted(scrollView);
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return _source.GetHeightForHeader(tableView, section);
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            return _source.GetViewForHeader(tableView, section);
        }

        public override void RowDeselected(UITableView tableView, NSIndexPath indexPath)
        {
            _source.RowDeselected(tableView, indexPath);
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            _source.RowSelected(tableView, indexPath);
        }

        public override void Scrolled(UIScrollView scrollView)
        {
            _source.Scrolled(scrollView);
            (_element as ExListView).InvokeScrollChanged(scrollView.ContentOffset.Y, scrollView.ContentOffset.X);
        }
    }
}