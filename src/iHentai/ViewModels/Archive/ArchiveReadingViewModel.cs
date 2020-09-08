using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using iHentai.ReadingImages;
using MimeTypes;
using SevenZip;

namespace iHentai.ViewModels.Archive
{
    class ArchiveReadingViewModel : ReadingViewModel
    {
        private readonly StorageFile _file;

        public ArchiveReadingViewModel(StorageFile file) : base(file.DisplayName)
        {
            _file = file;
            Init();
        }

        private void Init()
        {
            Task.Run(async () =>
            {
                IsLoading = true;
                using var fileStream = await _file.OpenStreamForReadAsync();
                using var extractor = new SevenZipExtractor(fileStream);
                var files = new List<byte[]>();
                for (var index = 0; index < extractor.ArchiveFileData.Count; index++)
                {
                    var item = extractor.ArchiveFileData[index];
                    if (!item.Encrypted && !item.IsDirectory &&
                        MimeTypeMap.GetMimeType(Path.GetExtension(item.FileName)).StartsWith("image"))
                    {
                        using var memoryStream = new MemoryStream();
                        extractor.ExtractFile(index, memoryStream);
                        var data = memoryStream.GetBuffer();
                        files.Add(data);
                    }
                }

                return files.Select((it, index) => new ArchiveReadingImage(it, index)).ToList();
            }).ContinueWith(task =>
            {
                Images = task.Result;
                IsLoading = false;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }

    class ArchiveReadingImage : ReadingImageBase
    {
        private readonly byte[] _data;

        public ArchiveReadingImage(byte[] data, int index)
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
