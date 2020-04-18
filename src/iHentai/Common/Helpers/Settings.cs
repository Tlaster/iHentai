using iHentai.Services.Core;
using Microsoft.Toolkit.Uwp.Helpers;

namespace iHentai.Common.Helpers
{
    class Settings : IPreferences
    {
        private readonly LocalObjectStorageHelper _helper = new LocalObjectStorageHelper();
        public T Get<T>(string key, T defaultValue = default)
        {
            return _helper.Read(key, defaultValue);
        }

        public void Set<T>(string key, T value)
        {
            _helper.Save(key, value);
        }
    }
}
