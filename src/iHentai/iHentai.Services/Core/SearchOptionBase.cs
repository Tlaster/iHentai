using System.Collections.Generic;
using System.Linq;
using iHentai.Services.Core.Common;
using iHentai.Services.Core.Common.Attributes;

namespace iHentai.Services.Core
{
    public abstract class SearchOptionBase : AutoString
    {
        public abstract string Keyword { get; set; }
        protected override string Separator { get; } = "&";

        public virtual IDictionary<string, string> ToDictionary()
        {
            return GetType()
                .GetProperties()
                .ToDictionary(item => item.GetAttr(item.GetValue(this)).Key,
                    item => item.GetAttr(this).GetValue(item.GetValue(this)));
        }
    }
}