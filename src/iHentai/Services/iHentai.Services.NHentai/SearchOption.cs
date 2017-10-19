using System;
using iHentai.Services.Core;
using iHentai.Services.Core.Common;
using iHentai.Services.Core.Common.Attributes;

namespace iHentai.Services.NHentai
{
    public class SearchOption : SearchOptionBase
    {
        [StringValue("q")]
        public override string Keyword { get; set; }
    }
}