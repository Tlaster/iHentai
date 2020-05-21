using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using iHentai.Services.Core;
using LiteDB;

namespace iHentai.Downloader
{
    public enum DownloadState
    {
        Done,
        Downloading,
        Pending,
        Paused
    }

    internal interface IDownloadItem
    {
        int Id { get; }
        string Link { get; }
        string Title { get; }
        int Count { get; }
        int Progress { get; }
        string ExtraData { get; }
        DateTime CreatedAt { get; }
        DownloadState State { get; }
        Task DownloadAsync();
    }

    public class DownloadItem : IDownloadItem, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Link { get; }
        public string Title { get; set; }
        public int Count { get; set; }
        public int Progress { get; set; }

        public string ExtraData { get; set; }
        public DateTime CreatedAt { get; set; }
        public DownloadState State { get; set; }
        
        public async Task DownloadAsync()
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DownloadManager
    {
        private readonly ConcurrentQueue<DownloadItem> _items = new ConcurrentQueue<DownloadItem>();
        
        internal List<IApi> Apis { get; private set; }
        public bool IsBusy { get; private set; }

        public static DownloadManager Instance { get; } = new DownloadManager();

        private DownloadManager()
        {
            
        }

        public void Init(List<IApi> apis)
        {
            Apis = apis;
        }

        private List<DownloadItem> BuildDownloadList(params DirectoryInfo[] directory)
        {
            var result = new List<DownloadItem>();
            foreach (var item in directory)
            {
                using var db = new LiteDatabase("Filename=" + Path.Combine(item.FullName, "data.db"));
                var items = db.GetCollection<DownloadItem>().Find(it => it.State != DownloadState.Downloading);
                result.AddRange(items);
            }

            return result;
        }

        public void Add(DirectoryInfo directory, DownloadItem item)
        {
            using var db = new LiteDatabase("Filename=" + Path.Combine(directory.FullName, "data.db"));
            db.GetCollection<DownloadItem>().Insert(item);
            db.Commit();
            _items.Enqueue(item);
            Task.Run(DownloadAsync);
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