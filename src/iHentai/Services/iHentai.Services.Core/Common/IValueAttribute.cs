namespace iHentai.Services.Core.Common
{
    internal interface IValueAttribute
    {
        string Key { get; }
        string Separator { get; set; }
        string ToQueryString(object instance);
    }
}