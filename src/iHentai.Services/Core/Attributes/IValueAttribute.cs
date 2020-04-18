namespace iHentai.Services.Core.Attributes
{
    internal interface IValueAttribute
    {
        string Key { get; }
        string Separator { get; set; }
        string ToString(object instance);
        string GetValue(object instance);
    }
}