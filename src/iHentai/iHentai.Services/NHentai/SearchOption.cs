using iHentai.Services.Core;
using iHentai.Services.Core.Common.Attributes;

namespace iHentai.Services.NHentai
{
    public class SearchOption : SearchOptionBase
    {
        [StringValue("query")]
        public override string Keyword { get; set; }
    }
}