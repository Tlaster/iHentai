using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Notifications;
using LiteDB;
using Microsoft.Toolkit.Helpers;
using Microsoft.Toolkit.Uwp.Notifications;

namespace iHentai.Common.Helpers
{
    enum DownloadState
    {
        Done,
        Downloading,
        Pending,
        Paused
    }

    interface IDownloadItem
    {
        int Id { get; }
        string Title { get; }
        int Count { get; }
        int Progress { get; }
        Task DownloadAsync();
        string ExtraData { get; }
        DateTime CreatedAt { get; }
        DownloadState State { get; }
    }

    class DownloadItem : IDownloadItem, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
        public int Progress { get; set; }
        public Task DownloadAsync()
        {
            throw new NotImplementedException();
        }

        public string ExtraData { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DownloadState State { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
    class DownloadManager
    {
        private readonly ConcurrentQueue<IDownloadItem> _items = new ConcurrentQueue<IDownloadItem>();
        private readonly ConcurrentDictionary<string, StorageFolder> _library = new ConcurrentDictionary<string, StorageFolder>();
        public bool IsBusy { get; private set; } = false;

        public async Task InitializationAsync()
        {
            var downloadLibrary = Singleton<Settings>.Instance.Get("download_library", new List<string>());
            if (!downloadLibrary.Any())
            {
                var folder = await DownloadsFolder.CreateFolderAsync("iHentai", CreationCollisionOption.OpenIfExists);
                AddLibraryFolder(folder);
            }
            else
            {
                foreach (var item in downloadLibrary)
                {
                    var folder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(item);
                    _library.TryAdd(item, folder);
                }
            }

            BuildDownloadList();
        }

        private void BuildDownloadList()
        {
            foreach (var (key, value) in _library)
            {
                using var db = new LiteDatabase(Path.Combine(value.Path, "data.db"));
                
            }
        }

        public void AddLibraryFolder(StorageFolder folder)
        {
            var downloadLibrary = Singleton<Settings>.Instance.Get("download_library", new List<string>());
            var token = StorageApplicationPermissions.FutureAccessList.Add(folder);
            downloadLibrary.Add(token);
            Singleton<Settings>.Instance.Set("download_library", downloadLibrary);
            _library.TryAdd(token ,folder);
        }

        public void RemoveLibraryFolder(StorageFolder folder)
        {
            // TODO: stop any download within ths folder
            var downloadLibrary = Singleton<Settings>.Instance.Get("download_library", new List<string>());
            var item = _library.FirstOrDefault(it => it.Value == folder);
            if (!string.IsNullOrEmpty(item.Key))
            {
                downloadLibrary.Remove(item.Key);
                Singleton<Settings>.Instance.Set("download_library", downloadLibrary);
                _library.TryRemove(item.Key, out _);
            }
        }

        public void Add(IDownloadItem item)
        {
            _items.Enqueue(item);
        }

        public async Task DownloadAsync()
        {
            if (IsBusy || !_items.Any())
            {
                return;
            }

            IsBusy = true;

            while (_items.Any())
            {
                if (_items.TryDequeue(out var item))
                {
                    await item.DownloadAsync();
                }
            }

            IsBusy = false;
        }
    }
}
