using System.Collections.Generic;
using System.Linq;
using iHentai.Apis.Core.Common;
using iHentai.Apis.Core.Common.Attributes;

namespace iHentai.Apis.Core
{
    public enum SearchTypes
    {
        Keyword,
        Tag
    }

    public abstract class SearchOptionBase : AutoString
    {
        public abstract string Keyword { get; set; }

        protected override string Separator { get; } = "&";

        public SearchTypes SearchType { get; set; } = SearchTypes.Keyword;

        public virtual IEnumerable<(string Key, string Value)> ToDictionary()
        {
            return GetType()
                .GetProperties()
                .Where(item =>
                    item.GetCustomAttributesData()
                        .Any(attr => typeof(IValueAttribute).IsAssignableFrom(attr.AttributeType)))
                .Select(item => (item.GetAttr().Key, item.GetAttr().GetValue(item.GetValue(this))));
        }
    }
}