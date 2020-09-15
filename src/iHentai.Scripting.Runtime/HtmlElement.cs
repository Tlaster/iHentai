using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace iHentai.Scripting.Runtime
{
    public sealed class HtmlElement
    {
        private readonly IElement? _element;

        private HtmlElement(IElement element)
        {
            _element = element;
        }

        public static HtmlElement parse(string content)
        {
            var parser = new HtmlParser();
            var doc = parser.ParseDocument(content);
            return new HtmlElement(doc.DocumentElement);
        }

        public HtmlElement? querySelector(string selector)
        {
            if (_element == null)
            {
                return null;
            }

            var item = _element.QuerySelector(selector);
            return item == null ? null : new HtmlElement(item);
        }

        public IReadOnlyList<HtmlElement>? querySelectorAll(string selector)
        {
            return _element?.QuerySelectorAll(selector)?.Select(it => new HtmlElement(it))?.ToList();
        }

        public string? text()
        {
            return _element?.Text();
        }

        public string? attr(string attr)
        {
            if (_element == null)
            {
                return null;
            }
            if (!_element.HasAttribute(attr))
            {
                return null;
            }
            return _element?.GetAttribute(attr);
        }
    }
}