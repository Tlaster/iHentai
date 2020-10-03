using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Newtonsoft.Json;

namespace iHentai.Common.Helpers
{
    internal class ScriptImageListCache : CacheBase2<List<string>>
    {
        public static ScriptImageListCache Instance { get; } = new ScriptImageListCache();
        protected override async Task<List<string>> InitializeTypeAsync(Stream stream, List<KeyValuePair<string, object>> initializerKeyValues = null)
        {
            var reader = new StreamReader(stream);
            return JsonConvert.DeserializeObject<List<string>>(await reader.ReadToEndAsync());
        }

        protected override async Task<List<string>> InitializeTypeAsync(StorageFile baseFile, List<KeyValuePair<string, object>> initializerKeyValues = null)
        {
            using var stream = await baseFile.OpenStreamForReadAsync();
            return await InitializeTypeAsync(stream, initializerKeyValues);
        }
    }
}