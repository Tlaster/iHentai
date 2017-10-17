using System;
using iHentai.Services.Core;
using iHentai.Services.Core.Common;

namespace iHentai.Services.NHentai
{
    public class SearchOption : ISearchOption
    {
        [Query("q")]
        public string Keyword { get; set; }

        public string ToQueryString()
        {
            return $"q={Keyword}";
        }
    }
}