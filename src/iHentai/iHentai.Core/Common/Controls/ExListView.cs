using System;
using Xamarin.Forms;

namespace iHentai.Core.Common.Controls
{
    public class ExListView : ListView
    {
        public event EventHandler LoadMoreRequest;
        public void RequestLoadMore()
        {
            LoadMoreRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}