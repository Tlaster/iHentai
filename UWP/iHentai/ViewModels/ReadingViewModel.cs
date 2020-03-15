using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace iHentai.ViewModels
{
    internal interface IReadingImage : INotifyPropertyChanged
    {
        ImageSource Source { get; }
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

        private async Task Reload(bool removeCache)
        {
            IsLoading = true;
            _source = await LoadImage(removeCache, _cancellationTokenSource.Token);
            IsLoading = false;
            OnPropertyChanged(nameof(Source));
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

    internal abstract class ReadingViewModel : TabViewModelBase
    {
        public List<IReadingImage> Images { get; protected set; }
    }
}