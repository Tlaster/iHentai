using AngleSharp.Dom;

namespace Html2Markdown
{
	internal static class HtmlAgilityPackExtensions
	{
		public static string GetAttributeOrEmpty(this IElement element, string attributeName)
		{
			return element.HasAttribute(attributeName) ? element.Attributes[attributeName].Value : "";
		}
	}
}