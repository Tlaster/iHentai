using System;
using System.Collections.Generic;
using System.Text;

namespace iHentai.Shared.Extensions
{
    public interface ISettings
    {
        T Get<T>(string key, T defaultValue = default);
        bool Set<T>(string key, T setValue);
        void Remove(string key);
        bool Contains(string key);
    }
}
