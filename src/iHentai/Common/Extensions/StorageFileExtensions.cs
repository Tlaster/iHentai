using System;
using System.IO;
using Windows.Storage;
using iHentai.ViewModels.Archive;

namespace iHentai.Common.Extensions
{
    internal static class StorageFileExtensions
    {
        public static IFileReadingViewModel GetFileReadingViewModel(this StorageFile file)
        {
            if (Path.GetExtension(file.Name).Contains("pdf", StringComparison.InvariantCultureIgnoreCase))
            {
                return new PdfReadingViewModel(file);
            }
            else
            {
                return new ArchiveReadingViewModel(file);
            }
        }
    }
}