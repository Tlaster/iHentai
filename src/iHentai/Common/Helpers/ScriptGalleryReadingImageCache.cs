using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace iHentai.Common.Helpers
{
    internal class ScriptGalleryReadingImageCache : CacheBase2<string>
    {
        public static ScriptGalleryReadingImageCache Instance { get; } = new ScriptGalleryReadingImageCache();
        protected override async Task<string> InitializeTypeAsync(Stream stream, List<KeyValuePair<string, object>> initializerKeyValues = null)
        {
            var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        protected override async Task<string> InitializeTypeAsync(StorageFile baseFile, List<KeyValuePair<string, object>> initializerKeyValues = null)
        {
            using var stream = await baseFile.OpenStreamForReadAsync();
            return await InitializeTypeAsync(stream, initializerKeyValues);
        }
    }
}