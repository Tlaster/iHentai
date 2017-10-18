using System;

namespace iHentai.Services.Core.Common
{
    public class QueryAttribute : Attribute
    {
        public QueryAttribute(string key)
        {
            Key = key;
        }

        public string Key { get; }
        public bool HasValue { get; set; } = true;
    }
}