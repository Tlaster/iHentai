using System.IO;
using iHentai.Data.Models;
using iHentai.Platform;
using LiteDB;
using Microsoft.Toolkit.Uwp.Helpers;

namespace iHentai.Data
{
    public class SettingsDb
    {
        private readonly LocalObjectStorageHelper _helper = new LocalObjectStorageHelper();
        public static SettingsDb Instance { get; } = new SettingsDb();

        public void Set<T>(string key, T value)
        {
            _helper.Save(key, value);
        }

        public T Get<T>(string key, T defaultValue = default)
        {
            return _helper.Read(key, defaultValue);
        }
    }
}