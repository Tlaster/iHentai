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

        public virtual IDictionary<string, string> ToDictionary()
        {
            return GetType()
                .GetProperties()
                .Where(item =>
                    item.GetCustomAttributesData()
                        .Any(attr => typeof(IValueAttribute).IsAssignableFrom(attr.AttributeType)))
                .ToDictionary(item => item.GetAttr().Key,
                    item => item.GetAttr().GetValue(item.GetValue(this)));
        }
    }
}