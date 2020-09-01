using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.Toolkit.Helpers;
using Microsoft.Toolkit.Uwp.UI;

namespace iHentai.Common.Helpers
{
    //class HtmlCache<T> : CacheBase<T>
    //{
    //    public HtmlCache()
    //    {
    //        CacheDuration = TimeSpan.FromMinutes(5);
    //        InitializeAsync(httpMessageHandler: Singleton<HentaiHttpMessageHandler>.Instance);
    //    }

    //    protected override Task<T> InitializeTypeAsync(Stream stream, List<KeyValuePair<string, object>> initializerKeyValues = null)
    //    {
    //        return Task.FromResult(HtmlConvert.DeserializeObject<T>(stream));
    //    }

    //    protected override async Task<T> InitializeTypeAsync(StorageFile baseFile, List<KeyValuePair<string, object>> initializerKeyValues = null)
    //    {
    //        using var stream = await baseFile.OpenStreamForReadAsync().ConfigureAwait(false);
    //        return await InitializeTypeAsync(stream, initializerKeyValues).ConfigureAwait(false);
    //    }
    //}
}
