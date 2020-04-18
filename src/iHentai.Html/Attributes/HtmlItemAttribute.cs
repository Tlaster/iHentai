using System;

namespace iHentai.Html.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class HtmlItemAttribute : Attribute, IHtmlItem
    {
        public HtmlItemAttribute(string selector)
        {
            Selector = selector;
        }

        public string Selector { get; }
        public string Attr { get; set; }
        public string RegexPattern { get; set; }
        public int RegexGroup { get; set; }
        public bool RawHtml { get; set; }
    }
}