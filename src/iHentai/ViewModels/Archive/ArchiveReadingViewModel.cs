using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using iHentai.Common;
using iHentai.Data;
using iHentai.Data.Models;
using iHentai.ReadingImages;
using MimeTypes;
using SevenZip;

namespace iHentai.ViewModels.Archive
{
    class ArchiveReadingViewModel : ReadingViewModel, IFileReadingViewModel
    {
        public StorageFile File { get; }

        public ArchiveReadingViewModel(StorageFile file) : base(file.DisplayName)
        {
            File = file;
        }

        public override void SaveReadingHistory()
        {
            
        }

        protected override int RestoreReadingProgress()
        {
            return 0;
        }

        protected override async Task<IEnumerable<IReadingImage>> InitImages()
        {
            
            using var fileStream = await File.OpenStreamForReadAsync();
            using var extractor = new SevenZipExtractor(fileStream);
            var files = new List<byte[]>();
            for (var index = 0; index < extractor.ArchiveFileData.Count; index++)
            {
                var item = extractor.ArchiveFileData[index];
                if (!item.Encrypted && !item.IsDirectory &&
                    MimeTypeMap.GetMimeType(Path.GetExtension(item.FileName)).StartsWith("image"))
                {
                    using var memoryStream = new MemoryStream();
                    //NEVER USE ASYNC FUNCTION HERE
                    extractor.ExtractFile(index, memoryStream);
                    var data = memoryStream.GetBuffer();
                    files.Add(data);
                }
            }
            return files.Select((it, index) => new BytesReadingImage(it, index)).ToList();
        }
    }
}
