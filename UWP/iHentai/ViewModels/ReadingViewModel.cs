using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using iHentai.Common.Helpers;
using Microsoft.Toolkit.Helpers;
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

    internal abstract class ReadingImageBase : IReadingImage
    {
        private ImageSource _source;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
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

        public bool IsLoading { get; protected set; }

        public async Task Reload()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            await Reload(true);
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
        public int SelectedIndex { get; set; }
        public List<IReadingImage> Images { get; protected set; }
        [DependsOn(nameof(Images))]
        public int Count => (Images?.Count ?? 1) - 1;

        public ReadingViewMode ViewMode
        {
            get => Singleton<Settings>.Instance.Get("reading_mode", ReadingViewMode.Flip);
            set
            {
                Singleton<Settings>.Instance.Set("reading_mode", value);
                OnPropertyChanged(nameof(ViewMode));
            }
        }
        
        [DependsOn(nameof(ViewMode))]
        public bool IsBookMode => ViewMode == ReadingViewMode.Book;
        [DependsOn(nameof(ViewMode))]
        public bool IsFlipMode => ViewMode == ReadingViewMode.Flip;

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