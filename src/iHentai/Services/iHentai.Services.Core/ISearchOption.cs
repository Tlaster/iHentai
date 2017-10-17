namespace iHentai.Services.Core
{
    public interface ISearchOption
    {
        string Keyword { get; set; }
        string ToQueryString();
    }
}