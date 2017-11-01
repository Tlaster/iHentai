using Windows.UI.Xaml.Controls;
using iHentai.Core.Common.Controls;
using iHentai.UWP.Renderers;
using Xamarin.Forms.Platform.UWP;
using ListView = Xamarin.Forms.ListView;

[assembly: ExportRenderer(typeof(ExListView), typeof(ExListViewRenderer))]

namespace iHentai.UWP.Renderers
{
    public class ExListViewRenderer : Xamarin.Forms.Platform.UWP.ListViewRenderer
    {
        private ScrollViewer _scrollViewer;

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (List != null)
                {
                    List.Loaded += List_Loaded;
                }
            }
            else if (e.OldElement != null)
            {
                if (_scrollViewer != null)
                    _scrollViewer.ViewChanged -= OnViewChanged;
            }
        }

        private void List_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            List.Loaded -= List_Loaded;
            _scrollViewer = List.GetScrollViewer();
            if (_scrollViewer != null)
            {
                _scrollViewer.ViewChanged += OnViewChanged;
            }
        }

        private void OnViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            (Element as ExListView).InvokeScrollChanged(_scrollViewer.VerticalOffset, _scrollViewer.HorizontalOffset);
            if (_scrollViewer.VerticalOffset == _scrollViewer.ScrollableHeight)
                (Element as ExListView).RequestLoadMore();
        }
    }
}