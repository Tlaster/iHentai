using iHentai.Apis.Core;
using iHentai.Apis.Core.Common.Attributes;

namespace iHentai.Apis.NHentai
{
    public class SearchOption : SearchOptionBase
    {
        [StringValue("query")]
        public override string Keyword { get; set; }

        public bool SortEnabled { get; set; }
    }
}