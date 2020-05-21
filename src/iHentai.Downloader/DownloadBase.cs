using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace iHentai.Downloader
{
    public abstract class DownloadBase<T>
    {
        private readonly ConcurrentDictionary<string, ConcurrentRequest> _concurrentTasks =
            new ConcurrentDictionary<string, ConcurrentRequest>();

        private HttpClient _httpClient;

        protected DownloadBase()
        {
            RetryCount = 1;
        }

        /// <summary>
        ///     Gets or sets the number of retries trying to ensure the file is cached.
        /// </summary>
        public uint RetryCount { get; set; }


        /// <summary>
        ///     Gets instance of <see cref="HttpClient" />
        /// </summary>
        protected HttpClient HttpClient
        {
            get
            {
                if (_httpClient == null)
                {
                    var messageHandler = new HttpClientHandler {MaxConnectionsPerServer = 20};

                    _httpClient = new HttpClient(messageHandler);
                }

                return _httpClient;
            }
        }

        public virtual void Initialize(HttpMessageHandler httpMessageHandler = null)
        {
            if (httpMessageHandler != null)
            {
                _httpClient = new HttpClient(httpMessageHandler);
            }
        }

        /// <summary>
        ///     Clears all files in the cache
        /// </summary>
        /// <returns>awaitable task</returns>
        public async Task ClearAsync(DirectoryInfo folder)
        {
            var files = folder.GetFiles();
            await InternalClearAsync(files).ConfigureAwait(false);
        }

        public async Task RemoveAsync(DirectoryInfo folder, params string[] downloadKeys)
        {
            if (downloadKeys == null || !downloadKeys.Any())
            {
                return;
            }
            
            var files = folder.GetFiles();

            var filesToDelete = new List<FileInfo>();

            var hashDictionary = files.ToDictionary(file => file.Name);

            foreach (var key in downloadKeys)
            {
                var fileName = key;

                if (hashDictionary.TryGetValue(fileName, out var file))
                {
                    filesToDelete.Add(file);
                }
            }

            await InternalClearAsync(filesToDelete).ConfigureAwait(false);
        }

        public Task PreDownloadAsync(Uri uri, DirectoryInfo folder, string key, bool throwOnError = false,
            CancellationToken cancellationToken = default)
        {
            return GetItemAsync(uri, folder, key, throwOnError, false, cancellationToken, null);
        }

        public Task<T> GetFromDownloadAsync(Uri uri, DirectoryInfo folder, string key, bool throwOnError = false,
            CancellationToken cancellationToken = default,
            List<KeyValuePair<string, object>> initializerKeyValues = null)
        {
            return GetItemAsync(uri, folder, key, throwOnError, false, cancellationToken, initializerKeyValues);
        }

        public async Task<FileInfo> GetFileFromDownloadedAsync(DirectoryInfo folder, string key)
        {
            return folder.GetFiles().FirstOrDefault(it => it.Name == key);
        }

        /// <summary>
        ///     Cache specific hooks to process items from HTTP response
        /// </summary>
        /// <param name="stream">input stream</param>
        /// <param name="initializerKeyValues">key value pairs used when initializing instance of generic type</param>
        /// <returns>awaitable task</returns>
        protected abstract Task<T> InitializeTypeAsync(Stream stream,
            List<KeyValuePair<string, object>> initializerKeyValues = null);

        /// <summary>
        ///     Cache specific hooks to process items from HTTP response
        /// </summary>
        /// <param name="baseFile">storage file</param>
        /// <param name="initializerKeyValues">key value pairs used when initializing instance of generic type</param>
        /// <returns>awaitable task</returns>
        protected abstract Task<T> InitializeTypeAsync(FileInfo baseFile,
            List<KeyValuePair<string, object>> initializerKeyValues = null);


        private async Task<T> GetItemAsync(Uri uri, DirectoryInfo folder, string key, bool throwOnError, bool preDownloadOnly,
            CancellationToken cancellationToken, List<KeyValuePair<string, object>> initializerKeyValues)
        {
            var instance = default(T);

            var fileName = key;

            _concurrentTasks.TryGetValue(fileName, out var request);

            // if similar request exists check if it was preDownloadOnly and validate that current request isn't preDownloadOnly
            if (request != null && request.EnsureDownloadedCopy && !preDownloadOnly)
            {
                await request.Task.ConfigureAwait(false);
                request = null;
            }

            if (request == null)
            {
                request = new ConcurrentRequest
                {
                    Task = GetFromDownloadedOrDownloadAsync(uri, folder, fileName, preDownloadOnly, cancellationToken,
                        initializerKeyValues),
                    EnsureDownloadedCopy = preDownloadOnly
                };

                _concurrentTasks[fileName] = request;
            }

            try
            {
                instance = await request.Task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                if (throwOnError)
                {
                    throw;
                }
            }
            finally
            {
                _concurrentTasks.TryRemove(fileName, out request);
                request = null;
            }

            return instance;
        }

        private async Task<T> GetFromDownloadedOrDownloadAsync(Uri uri, DirectoryInfo folder, string fileName,
            bool preDownloadOnly, CancellationToken cancellationToken,
            List<KeyValuePair<string, object>> initializerKeyValues)
        {
            FileInfo baseFile = null;
            var instance = default(T);


            if (instance != null)
            {
                return instance;
            }

            baseFile = folder.GetFiles().FirstOrDefault(it => it.Name == fileName);

            var downloadDataFile = baseFile == null;

            if (baseFile == null)
            {
                baseFile = new FileInfo(Path.Combine(folder.FullName, fileName));
                baseFile.Create();
            }
            
            var cacheFile = await GetFromCache(uri, cancellationToken);

            if (cacheFile != null && downloadDataFile)
            {
                cacheFile.CopyTo(baseFile.FullName, true);
            } 
            else if (downloadDataFile)
            {
                uint retries = 0;
                try
                {
                    while (retries < RetryCount)
                    {
                        try
                        {
                            instance = await DownloadFileAsync(uri, baseFile, preDownloadOnly, cancellationToken,
                                initializerKeyValues).ConfigureAwait(false);

                            if (instance != null)
                            {
                                break;
                            }
                        }
                        catch (FileNotFoundException)
                        {
                        }

                        retries++;
                    }
                }
                catch (Exception)
                {
                    baseFile.Delete();
                    throw; // re-throwing the exception changes the stack trace. just throw
                }
            }

            if (EqualityComparer<T>.Default.Equals(instance, default) && !preDownloadOnly)
            {
                instance = await InitializeTypeAsync(baseFile, initializerKeyValues).ConfigureAwait(false);
            }

            return instance;
        }

        protected abstract Task<FileInfo> GetFromCache(Uri uri, CancellationToken cancellationToken);

        private async Task<T> DownloadFileAsync(Uri uri, FileInfo baseFile, bool preDownloadOnly,
            CancellationToken cancellationToken, List<KeyValuePair<string, object>> initializerKeyValues)
        {
            var instance = default(T);

            using (var ms = new MemoryStream())
            {
                using (var stream = await HttpClient.GetStreamAsync(uri))
                {
                    await stream.CopyToAsync(ms, 81920, cancellationToken);
                    ms.Flush();

                    ms.Position = 0;

                    using var fs = baseFile.OpenRead();
                    await ms.CopyToAsync(fs, 81920, cancellationToken);

                    await fs.FlushAsync(cancellationToken);

                    ms.Position = 0;
                }

                // if its pre-cache we aren't looking to load items in memory
                if (!preDownloadOnly)
                {
                    instance = await InitializeTypeAsync(ms, initializerKeyValues).ConfigureAwait(false);
                }
            }

            return instance;
        }

        private async Task InternalClearAsync(IEnumerable<FileInfo> files)
        {
            foreach (var file in files)
            {
                try
                {
                    file.Delete();
                }
                catch
                {
                    // Just ignore errors for now
                }
            }
        }

        //private async Task<DirectoryInfo> GetDownloadFolderAsync(string sub)
        //{
        //    if (string.IsNullOrEmpty(sub))
        //    {
        //        return _baseFolder;
        //    }

        //    var subFolder = await _baseFolder.GetFolderAsync(sub);
        //    return subFolder;

        //}

        private class ConcurrentRequest
        {
            public Task<T> Task { get; set; }

            public bool EnsureDownloadedCopy { get; set; }
        }
    }
}