namespace iHentai.Services.Core
{
    public interface IPreferences
    {
        T Get<T>(string key, T defaultValue = default);
        void Set<T>(string key, T value);
    }
}