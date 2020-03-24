using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.Toolkit.Helpers;

namespace iHentai.Common.Helpers
{
    interface IDownloadItem
    {
        int Count { get; }
        int Progress { get; }
        Task DownloadAsync();
        string ExtraData { get; }
    }

    class DownloadManager
    {
        private readonly List<IDownloadItem> _items = new List<IDownloadItem>();
        public async Task InitializationAsync()
        {
            var folder = await DownloadsFolder.CreateFolderAsync("iHentai", CreationCollisionOption.OpenIfExists);
            Singleton<ImageDownload>.Instance.Initialize(folder, Singleton<HentaiHttpMessageHandler>.Instance);
        }

        public void Add(IDownloadItem item)
        {
            _items.Add(item);
            item.DownloadAsync().ContinueWith(_ => _items.Remove(item));
        }
    }
}
