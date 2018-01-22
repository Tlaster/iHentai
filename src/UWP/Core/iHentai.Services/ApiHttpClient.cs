using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using FluentScheduler;
using Flurl.Http.Configuration;
using iHentai.Basic.Helpers;
using Microsoft.Toolkit.Uwp.Helpers;

namespace iHentai.Services
{
    public class ApiHttpClientFactory : DefaultHttpClientFactory
    {
        public override HttpMessageHandler CreateMessageHandler()
        {
            return Singleton<ApiHttpClient>.Instance;
        }
    }


    public class CacheManager : IJob
    {
        private readonly StorageFolder _cacheFolder = ApplicationData.Current.LocalCacheFolder;


        private readonly ConcurrentDictionary<string, CacheModel<byte[]>> _memoryCache =
            new ConcurrentDictionary<string, CacheModel<byte[]>>();

        public CacheManager()
        {
            Singleton<JobRegistry>.Instance.Schedule(this).ToRunEvery(5).Minutes();
        }

        public void Execute()
        {
            CleanMemory().Wait();
        }

        public async Task<bool> Contains(string key)
        {
            var base64 = key.Base64();
            return _memoryCache.ContainsKey(base64) || await _cacheFolder.FileExistsAsync(base64);
        }

        public void Put(string key, byte[] data, TimeSpan maxAge)
        {
            var base64 = key.Base64();
            _memoryCache.AddOrUpdate(base64, new CacheModel<byte[]>(DateTime.UtcNow, data, maxAge), (s, model) =>
            {
                model.Data = data;
                model.LastUsed = DateTime.UtcNow;
                return model;
            });
        }

        public async Task<byte[]> Get(string key, TimeSpan maxAge)
        {
            var base64 = key.Base64();
            if (_memoryCache.ContainsKey(base64) && _memoryCache.TryGetValue(base64, out var value))
            {
                value.LastUsed = DateTime.UtcNow;
                return value.Data;
            }

            var file = await _cacheFolder.GetFileAsync(base64);
            var bytes = await file.ReadBytesAsync();
            _memoryCache.TryAdd(base64, new CacheModel<byte[]>(DateTime.UtcNow, bytes, maxAge));
            return bytes;
        }

        private async Task CleanMemory()
        {
            var items = _memoryCache.AsParallel()
                .Where(item => item.Value.LastUsed > DateTime.UtcNow - item.Value.MaxAge).ToList();
            await Task.WhenAll(items.Select(async item =>
            {
                var file = await _cacheFolder.CreateFileAsync(item.Key, CreationCollisionOption.ReplaceExisting);
                await File.WriteAllBytesAsync(file.Path, item.Value.Data);
                _memoryCache.TryRemove(item.Key, out var _);
            }));
        }

        public async Task CleanDisk()
        {
            var files = await _cacheFolder.GetFilesAsync();
            foreach (var file in files)
            {
                if (file.DateCreated > DateTimeOffset.Now - TimeSpan.FromDays(1)) await file.DeleteAsync();
            }
        }

        public class CacheModel<T>
        {
            //public CacheModel(DateTime lastUsed, T data) : this(lastUsed, data, TimeSpan.FromMinutes(30))
            //{
            //}

            public CacheModel(DateTime lastUsed, T data, TimeSpan maxAge)
            {
                MaxAge = maxAge;
                LastUsed = lastUsed;
                Data = data;
            }

            public TimeSpan MaxAge { get; set; }
            public DateTime LastUsed { get; set; }
            public T Data { get; set; }
        }
    }

    public static class StringExtensions
    {
        public static string Base64(this string value)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        }
    }

    public class ApiHttpClient : HttpClientHandler
    {
        private const int DefaultCacheAge = 10;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            CookieContainer.GetCookies(request.RequestUri)
                .Cast<Cookie>()
                .ToList()
                .ForEach(c => c.Expired = true);
            Singleton<ApiContainer>.Instance.InstanceDatas.Values.FirstOrDefault(item =>
                item is IHttpHandler handler && handler.Handle(ref request));
            var key = request.RequestUri.ToString();
            if (request.Headers.CacheControl != null && !request.Headers.CacheControl.NoCache)
            {
                if (await Singleton<CacheManager>.Instance.Contains(key))
                {
                    var age = request.Headers.CacheControl.MaxAge ?? TimeSpan.FromMinutes(DefaultCacheAge);
                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ByteArrayContent(await Singleton<CacheManager>.Instance.Get(key, age)),
                        RequestMessage = request,
                        Version = request.Version
                    };
                }
            }
            try
            {
                var result = await base.SendAsync(request, cancellationToken);
                if (request.Headers.CacheControl != null && !request.Headers.CacheControl.NoCache)
                {
                    var age = request.Headers.CacheControl.MaxAge ?? TimeSpan.FromMinutes(DefaultCacheAge);
                    var bytes = await result.Content.ReadAsByteArrayAsync();
                    Singleton<CacheManager>.Instance.Put(key, bytes, age);
                }
                return result;
            }
            catch (Exception e) when (e is HttpRequestException || e is WebException || e is WebSocketException)
            {
                if (await Singleton<CacheManager>.Instance.Contains(key))
                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ByteArrayContent(await Singleton<CacheManager>.Instance.Get(key, TimeSpan.FromMinutes(DefaultCacheAge))),
                        RequestMessage = request,
                        Version = request.Version
                    };

                throw;
            }
        }
    }
}