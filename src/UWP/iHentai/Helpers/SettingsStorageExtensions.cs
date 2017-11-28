using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace iHentai.Helpers
{
    // Use these extension methods to store and retrieve local and roaming app data
    // For more info regarding storing and retrieving app data see documentation at
    // https://docs.microsoft.com/windows/uwp/app-settings/store-and-retrieve-app-data
    public static class SettingsStorageExtensions
    {
        private const string FileExtension = ".json";

        public static bool IsRoamingStorageAvailable(this ApplicationData appData)
        {
            return appData.RoamingStorageQuota == 0;
        }

        public static async Task SaveAsync<T>(this StorageFolder folder, string name, T content)
        {
            var file = await folder.CreateFileAsync(GetFileName(name), CreationCollisionOption.ReplaceExisting);
            var fileContent = content.ToJson();

            await FileIO.WriteTextAsync(file, fileContent);
        }

        public static async Task<T> ReadAsync<T>(this StorageFolder folder, string name)
        {
            if (!File.Exists(Path.Combine(folder.Path, GetFileName(name))))
                return default;

            var file = await folder.GetFileAsync($"{name}.json");
            var fileContent = await FileIO.ReadTextAsync(file);

            return fileContent.JsonTo<T>();
        }

        public static T Read<T>(this string key, T defaultValue = default)
        {
            return ApplicationData.Current.LocalSettings.Values.TryGetValue(key, out var obj)
                ? ((string) obj).JsonTo<T>()
                : defaultValue;
        }


        public static void Save<T>(this T obj, string key)
        {
            ApplicationData.Current.LocalSettings.Values[key] = obj.ToJson();
        }


        public static T Read<T>(this (string Container, string Key) value, T defaultValue = default)
        {
            return ApplicationData.Current.LocalSettings
                .CreateContainer(value.Container, ApplicationDataCreateDisposition.Always)
                .Values.TryGetValue(value.Key, out var obj)
                ? ((string) obj).JsonTo<T>()
                : defaultValue;
        }


        public static void Save<T>(this T obj, string container, string key)
        {
            ApplicationData.Current.LocalSettings
                .CreateContainer(container, ApplicationDataCreateDisposition.Always)
                .Values[key] = obj.ToJson();
        }


        public static async Task<StorageFile> SaveFileAsync(this StorageFolder folder, byte[] content, string fileName,
            CreationCollisionOption options = CreationCollisionOption.ReplaceExisting)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("ExceptionSettingsStorageExtensionsFileNameIsNullOrEmpty".GetLocalized(),
                    nameof(fileName));

            var storageFile = await folder.CreateFileAsync(fileName, options);
            await FileIO.WriteBytesAsync(storageFile, content);
            return storageFile;
        }

        public static async Task<byte[]> ReadFileAsync(this StorageFolder folder, string fileName)
        {
            var item = await folder.TryGetItemAsync(fileName).AsTask().ConfigureAwait(false);

            if (item != null && item.IsOfType(StorageItemTypes.File))
            {
                var storageFile = await folder.GetFileAsync(fileName);
                var content = await storageFile.ReadBytesAsync();
                return content;
            }

            return null;
        }

        public static async Task<byte[]> ReadBytesAsync(this StorageFile file)
        {
            if (file != null)
                using (IRandomAccessStream stream = await file.OpenReadAsync())
                {
                    using (var reader = new DataReader(stream.GetInputStreamAt(0)))
                    {
                        await reader.LoadAsync((uint) stream.Size);
                        var bytes = new byte[stream.Size];
                        reader.ReadBytes(bytes);
                        return bytes;
                    }
                }

            return null;
        }

        private static string GetFileName(string name)
        {
            return string.Concat(name, FileExtension);
        }
    }
}