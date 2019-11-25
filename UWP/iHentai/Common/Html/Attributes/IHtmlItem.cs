namespace iHentai.Common.Html.Attributes
{
    internal interface IHtmlItem
    {
        string Selector { get; }
        string Attr { get; }
        string RegexPattern { get; }
        int RegexGroup { get; }
    }
}