namespace iHentai.Apis.Core.Common
{
    public interface ISettings
    {
        T Get<T>(string key, T defaultValue = default);
        bool Set<T>(string key, T setValue);
        void Remove(string key);
        bool Contains(string key);
    }
}