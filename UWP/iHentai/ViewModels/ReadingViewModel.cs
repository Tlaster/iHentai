using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using iHentai.Common.Helpers;
using Microsoft.Toolkit.Helpers;
using Microsoft.Toolkit.Uwp.UI;
using PropertyChanged;

namespace iHentai.ViewModels
{
    internal interface IReadingImage : INotifyPropertyChanged
    {
        ImageSource Source { get; }
        int Index { get; }
        bool IsLoading { get; }
        Task Reload();
    }


    internal class ReadingImage : ReadingImageBase
    {
        private readonly string _name;

        public ReadingImage(string url, int index, string name)
        {
            Url = url;
            _name = name;
            Index = index;
        }

        public string Url { get; }

        protected override async Task<ImageSource> LoadImage(bool removeCache, CancellationToken token)
        {
            if (removeCache)
            {
                await ImageCache.Instance.RemoveAsync(new[] {new Uri(Url)});
            }

            //TODO: check if image already downloaded
            return await ImageCache.Instance.GetFromCacheAsync(new Uri(Url), cancellationToken: token);
        }
    }

    internal abstract class ReadingImageBase : IReadingImage
    {
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private ImageSource _source;

        public ImageSource Source
        {
            get
            {
                if (_source == null)
                {
                    Reload(false);
                }

                return _source;
            }
        }

        public int Index { get; protected set; }

        public bool IsLoading { get; protected set; }

        public async Task Reload()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            await Reload(true);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private async Task Reload(bool removeCache)
        {
            if (IsLoading)
            {
                return;
            }

            IsLoading = true;
            _source = await LoadImage(removeCache, _cancellationTokenSource.Token);
            IsLoading = false;
            if (_source != null)
            {
                OnPropertyChanged(nameof(Source));
            }
            else
            {
                Reload(true);
            }
        }

        protected abstract Task<ImageSource> LoadImage(bool removeCache, CancellationToken token);

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    internal enum ReadingViewMode
    {
        Book,
        Flip
    }

    internal abstract class ReadingViewModel : TabViewModelBase
    {
        private int _selectedIndex;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (Images != null)
                {
                    _selectedIndex = value;
                    OnPropertyChanged(nameof(SelectedIndex));
                }
            }
        }

        public List<IReadingImage> Images { get; protected set; }

        [DependsOn(nameof(Images))] public int Count => (Images?.Count ?? 1) - 1;

        public bool CanSwitchChapter { get; protected set; } = true;

        public FlowDirection FlowDirection
        {
            get => Singleton<Settings>.Instance.Get("reading_flow_direction", FlowDirection.RightToLeft);
            set
            {
                Singleton<Settings>.Instance.Set("reading_flow_direction", value);
                OnPropertyChanged(nameof(FlowDirection));
            }
        }

        public ReadingViewMode ViewMode
        {
            get => Singleton<Settings>.Instance.Get("reading_mode", ReadingViewMode.Flip);
            set
            {
                Singleton<Settings>.Instance.Set("reading_mode", value);
                OnPropertyChanged(nameof(ViewMode));
            }
        }

        [DependsOn(nameof(FlowDirection))] public bool IsLTR => FlowDirection == FlowDirection.LeftToRight;

        [DependsOn(nameof(FlowDirection))] public bool IsRTL => FlowDirection == FlowDirection.RightToLeft;

        [DependsOn(nameof(ViewMode))] public bool IsBookMode => ViewMode == ReadingViewMode.Book;

        [DependsOn(nameof(ViewMode))] public bool IsFlipMode => ViewMode == ReadingViewMode.Flip;

        public void ReloadCurrent()
        {
            if (ViewMode == ReadingViewMode.Flip)
            {
                Images?.ElementAtOrDefault(SelectedIndex)?.Reload();
            }
            else if (ViewMode == ReadingViewMode.Book)
            {
                Images?.ElementAtOrDefault(SelectedIndex)?.Reload();
                Images?.ElementAtOrDefault(SelectedIndex + 1)?.Reload();
            }
        }
    }
}