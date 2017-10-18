using System.Linq;
using System.Reflection;
using iHentai.Services.Core.Common;

namespace iHentai.Services.Core
{
    public abstract class SearchOptionBase : AutoString
    {
        public abstract string Keyword { get; set; }
        protected override string Separator { get; } = "&";

    }
}