using NiL.JS.Core.Interop;

namespace iHentai.Extensions.Runtime
{
    public interface IExtensionStorage
    {
        void Set(string extensionId, string key, string value);
        string? Get(string extensionId, string key);
    }


    internal class LocalStorage
    {
        private readonly string _extensionId;
        private readonly IExtensionStorage _storage;

        public LocalStorage(string extensionId, IExtensionStorage storage)
        {
            _extensionId = extensionId;
            _storage = storage;
        }

        [DoNotDelete]
        public void setItem(string key, string value)
        {
            _storage.Set(_extensionId, key, value);
        }

        [DoNotDelete]
        public string? getItem(string key)
        {
            return _storage.Get(_extensionId, key);
        }
    }
}