using System;

namespace Html2Markdown.Replacement
{
    public class CustomReplacer : IReplacer
    {
        public Func<string, string> CustomAction { get; set; }

        public string Replace(string html)
        {
            return CustomAction.Invoke(html);
        }
    }
}