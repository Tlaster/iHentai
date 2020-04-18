using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using AngleSharp.Common;
using iHentai.Services.EHentai.Model;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Toolkit.Uwp.UI.Controls;

namespace iHentai.Controls
{
    internal class ImageEx2 : ImageEx
    {
        public static readonly DependencyProperty EhGalleryPageNormalImageProperty = DependencyProperty.Register(
            nameof(EhGalleryPageNormalImage), typeof(EHGalleryPageNormalImage), typeof(ImageEx2),
            new PropertyMetadata(default(EHGalleryPageNormalImage), PropertyChangedCallback));

        private CancellationTokenSource _tokenSource2;
        private string _url;

        public EHGalleryPageNormalImage EhGalleryPageNormalImage
        {
            get => (EHGalleryPageNormalImage) GetValue(EhGalleryPageNormalImageProperty);
            set => SetValue(EhGalleryPageNormalImageProperty, value);
        }


        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ImageEx2 view)
            {
                if (e.Property == EhGalleryPageNormalImageProperty)
                {
                    view.OnEhGalleryPageNormalImageChanged(e.NewValue as EHGalleryPageNormalImage);
                }
            }
        }

        private static async Task<BitmapImage> CropImageAsync(WriteableBitmap writeableBitmap,
            int startPointX, int startPointY, int width, int height)
        {
            using var stream = new InMemoryRandomAccessStream();
            await CropImageAsync(writeableBitmap, stream, new Rect(startPointX, startPointY, width, height));
            var img = new BitmapImage();
            await img.SetSourceAsync(stream);
            return img;
        }


        private static async Task CropImageAsync(WriteableBitmap writeableBitmap, IRandomAccessStream stream,
            Rect croppedRect)
        {
            croppedRect.X = Math.Max(croppedRect.X, 0);
            croppedRect.Y = Math.Max(croppedRect.Y, 0);
            var x = (uint) Math.Floor(croppedRect.X);
            var y = (uint) Math.Floor(croppedRect.Y);
            var width = (uint) Math.Floor(croppedRect.Width);
            var height = (uint) Math.Floor(croppedRect.Height);
            using var sourceStream = writeableBitmap.PixelBuffer.AsStream();
            var buffer = new byte[sourceStream.Length];
            await sourceStream.ReadAsync(buffer, 0, buffer.Length);
            var bitmapEncoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
            bitmapEncoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied,
                (uint) writeableBitmap.PixelWidth, (uint) writeableBitmap.PixelHeight, 96.0, 96.0, buffer);
            bitmapEncoder.BitmapTransform.Bounds = new BitmapBounds
            {
                X = x,
                Y = y,
                Width = width,
                Height = height
            };
            await bitmapEncoder.FlushAsync();
        }


        private async void OnEhGalleryPageNormalImageChanged(EHGalleryPageNormalImage newValue)
        {
            try
            {
                _tokenSource2?.Cancel();
                _tokenSource2 = new CancellationTokenSource();
                _url = newValue.Source;
                var props = new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("ImageWidth", newValue.ThumbWidth),
                    new KeyValuePair<string, object>("ImageHeight", newValue.ThumbHeight)
                };
                var img = await WriteableImageCache.Instance.GetFromCacheAsync(new Uri(newValue.Source), true,
                    _tokenSource2.Token,
                    props);
                var source = await CropImageAsync(img, Convert.ToInt32(Math.Abs(newValue.OffsetX)),
                    Convert.ToInt32(Math.Abs(newValue.OffsetY)), Convert.ToInt32(newValue.ThumbWidth),
                    Convert.ToInt32(newValue.ThumbHeight));
                lock (LockObj)
                {
                    if (_url == newValue.Source)
                    {
                        Source = source;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // nothing to do as cancellation has been requested.
            }
            catch (Exception e)
            {
            }
        }

        internal class WriteableImageCache : CacheBase<WriteableBitmap>
        {
            private const string DateAccessedProperty = "System.DateAccessed";
            private readonly List<string> _extendedPropertyNames = new List<string>();

            public WriteableImageCache()
            {
                _extendedPropertyNames.Add(DateAccessedProperty);
            }

            public static WriteableImageCache Instance { get; } = new WriteableImageCache();

            protected override async Task<WriteableBitmap> InitializeTypeAsync(Stream stream,
                List<KeyValuePair<string, object>> initializerKeyValues = null)
            {
                if (stream.Length == 0)
                {
                    throw new FileNotFoundException();
                }

                return await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                {
                    var dic = initializerKeyValues.ToDictionary(it => it.Key, it => it.Value);
                    var height = dic.TryGet("ImageHeight") as int? ?? 0d;
                    var width = dic.TryGet("ImageWidth") as int? ?? 0d;
                    var img = new WriteableBitmap(Convert.ToInt32(width), Convert.ToInt32(height));
                    await img.SetSourceAsync(stream.AsRandomAccessStream());
                    return img;
                });
            }

            protected override async Task<WriteableBitmap> InitializeTypeAsync(StorageFile baseFile,
                List<KeyValuePair<string, object>> initializerKeyValues = null)
            {
                using var stream = await baseFile.OpenStreamForReadAsync().ConfigureAwait(false);
                return await InitializeTypeAsync(stream, initializerKeyValues).ConfigureAwait(false);
            }

            protected override async Task<bool> IsFileOutOfDateAsync(StorageFile file, TimeSpan duration,
                bool treatNullFileAsOutOfDate = true)
            {
                if (file == null)
                {
                    return treatNullFileAsOutOfDate;
                }

                var extraProperties =
                    await file.Properties.RetrievePropertiesAsync(_extendedPropertyNames).AsTask()
                        .ConfigureAwait(false);

                var propValue = extraProperties[DateAccessedProperty];

                if (propValue is DateTimeOffset lastAccess)
                {
                    return DateTime.Now.Subtract(lastAccess.DateTime) > duration;
                }

                var properties = await file.GetBasicPropertiesAsync().AsTask().ConfigureAwait(false);

                return properties.Size == 0 || DateTime.Now.Subtract(properties.DateModified.DateTime) > duration;
            }
        }
    }
}