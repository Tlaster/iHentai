using iHentai.Shared.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace iHentai.Common.Controls
{
    public class ExListView : ListView
    {
        private Task _loadTask;

        public ExListView()
        {
            ItemAppearing += ExListView_ItemAppearing;
        }
        
        private void ExListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (ItemsSource is IList items && items.IndexOf(e.Item) == items.Count - 1)
            {
                if (ItemsSource is ISupportIncrementalLoading supportIncrementalLoading && supportIncrementalLoading.HasMoreItems)
                {
                    if (_loadTask == null || _loadTask?.Status != TaskStatus.Running)
                    {
                        _loadTask = supportIncrementalLoading.LoadMoreItemsAsync();
                        _loadTask.FireAndForget();
                    }
                }
            }
        }
    }
}
