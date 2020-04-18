using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Toolkit.Helpers;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.UI;

namespace iHentai.Common.Helpers
{
    class ImageDownload : DownloadBase<BitmapImage>
    {

        private const string DateAccessedProperty = "System.DateAccessed";

        private List<string> _extendedPropertyNames = new List<string>();

        public ImageDownload()
        {
            _extendedPropertyNames.Add(DateAccessedProperty);
        }

        /// <summary>
        /// Cache specific hooks to process items from HTTP response
        /// </summary>
        /// <param name="stream">input stream</param>
        /// <param name="initializerKeyValues">key value pairs used when initializing instance of generic type</param>
        /// <returns>awaitable task</returns>
        protected override async Task<BitmapImage> InitializeTypeAsync(Stream stream, List<KeyValuePair<string, object>> initializerKeyValues = null)
        {
            if (stream.Length == 0)
            {
                throw new FileNotFoundException();
            }

            return await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
            {
                BitmapImage image = new BitmapImage();

                if (initializerKeyValues != null && initializerKeyValues.Count > 0)
                {
                    foreach (var kvp in initializerKeyValues)
                    {
                        if (string.IsNullOrWhiteSpace(kvp.Key))
                        {
                            continue;
                        }

                        var propInfo = image.GetType().GetProperty(kvp.Key, BindingFlags.Public | BindingFlags.Instance);

                        if (propInfo != null && propInfo.CanWrite)
                        {
                            propInfo.SetValue(image, kvp.Value);
                        }
                    }
                }

                await image.SetSourceAsync(stream.AsRandomAccessStream()).AsTask().ConfigureAwait(false);

                return image;
            });
        }

        /// <summary>
        /// Cache specific hooks to process items from HTTP response
        /// </summary>
        /// <param name="baseFile">storage file</param>
        /// <param name="initializerKeyValues">key value pairs used when initializing instance of generic type</param>
        /// <returns>awaitable task</returns>
        protected override async Task<BitmapImage> InitializeTypeAsync(StorageFile baseFile, List<KeyValuePair<string, object>> initializerKeyValues = null)
        {
            using (var stream = await baseFile.OpenStreamForReadAsync().ConfigureAwait(false))
            {
                return await InitializeTypeAsync(stream, initializerKeyValues).ConfigureAwait(false);
            }
        }

        protected override async Task<StorageFile> GetFromCache(Uri uri, CancellationToken cancellationToken)
        {
            return await ImageCache.Instance.GetFileFromCacheAsync(uri) ?? await Singleton<ProgressImageCache>.Instance.GetFileFromCacheAsync(uri);
        }
    }
}
