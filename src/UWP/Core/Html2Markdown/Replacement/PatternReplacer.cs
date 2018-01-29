using System.Text.RegularExpressions;

namespace Html2Markdown.Replacement
{
    public class PatternReplacer : IReplacer
    {
        public string Pattern { get; set; }

        public string Replacement { get; set; }

        public string Replace(string html)
        {
            return Regex.Replace(html, Pattern, Replacement);
        }
    }
}