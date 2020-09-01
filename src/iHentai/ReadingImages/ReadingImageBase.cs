using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Media;

namespace iHentai.ReadingImages
{

    public interface IReadingImage : INotifyPropertyChanged
    {
        ImageSource Source { get; }
        int Index { get; }
        bool IsLoading { get; }
        float Progress { get; }
        Task Reload();
    }


    public class ReadingImage : ReadingImageBase
    {
        public ReadingImage(string url, int index)
        {
            Url = url;
            Index = index;
        }

        public string Url { get; }

        protected override async Task<ImageSource> LoadImage(bool removeCache, CancellationToken token)
        {
            throw new NotImplementedException();
            // if (removeCache)
            // {
            //     await Singleton<ProgressImageCache>.Instance.RemoveAsync(new[] {new Uri(Url)});
            // }
            //
            // //TODO: check if image already downloaded
            // return await Singleton<ProgressImageCache>.Instance.GetFromCacheAsync(new Uri(Url), cancellationToken: token, progress: this);
        }
    }

    public abstract class ReadingImageBase : IReadingImage, IProgress<float>
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

        public float Progress { get; protected set; }

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

        public void Report(float value)
        {
            CoreApplication.MainView.Dispatcher.RunAsync(priority: CoreDispatcherPriority.Normal,
                () => Progress = value);
        }
    }

}
