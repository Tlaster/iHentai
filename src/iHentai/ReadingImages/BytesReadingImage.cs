using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace iHentai.ReadingImages
{
    class BytesReadingImage : ReadingImageBase
    {
        private readonly byte[] _data;

        public BytesReadingImage(byte[] data, int index)
        {
            _data = data;
            Index = index;
        }

        protected override async Task<ImageSource> LoadImage(bool removeCache, CancellationToken token)
        {
            using var stream = new InMemoryRandomAccessStream();
            await stream.WriteAsync(_data.AsBuffer());
            stream.Seek(0);
            var bitmap = new BitmapImage();
            await bitmap.SetSourceAsync(stream);
            return bitmap;
        }
    }
}