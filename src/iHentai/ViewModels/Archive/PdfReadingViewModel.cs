using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Pdf;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using iHentai.ReadingImages;

namespace iHentai.ViewModels.Archive
{
    class PdfReadingViewModel : ReadingViewModel, IFileReadingViewModel
    {
        public PdfReadingViewModel(StorageFile file) : base(file.DisplayName)
        {
            File = file;
        }

        public StorageFile File { get; }

        protected override async Task<IEnumerable<IReadingImage>> InitImages()
        {
            var doc = await PdfDocument.LoadFromFileAsync(File);
            var result = new List<BytesReadingImage>();
            for (uint i = 0; i < doc.PageCount; i++)
            {
                using var page = doc.GetPage(i);
                using var stream = new InMemoryRandomAccessStream();
                await page.RenderToStreamAsync(stream);
                var bytes = new byte[stream.Size];
                using var inputStream = stream.GetInputStreamAt(0);
                using var dataReader = new DataReader(inputStream);
                await dataReader.LoadAsync(Convert.ToUInt32(stream.Size));
                dataReader.ReadBytes(bytes);
                result.Add(new BytesReadingImage(bytes, Convert.ToInt32(i)));
            }

            return result;
        }

    }
}
