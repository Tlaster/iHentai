using System;

namespace iHentai.Services
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ApiKeyAttribute : Attribute
    {
        public ApiKeyAttribute(string key)
        {
            Key = key;
        }

        public string Key { get; }
    }
}