using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace iHentai.ReadingImages
{
    public class LocalReadingImage : ReadingImageBase
    {
        private readonly string _path;

        public LocalReadingImage(string path, int index)
        {
            _path = path;
            Index = index;
        }

        protected override async Task<ImageSource> LoadImage(bool removeCache, CancellationToken token)
        {
            var file = await StorageFile.GetFileFromPathAsync(_path);
            using IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read);
            var bitmapImage = new BitmapImage();
            await bitmapImage.SetSourceAsync(fileStream);
            return bitmapImage;
        }
    }
}