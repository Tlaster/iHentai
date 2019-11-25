using System;

namespace iHentai.Common.Html.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class HtmlConverterAttribute : Attribute
    {
        public HtmlConverterAttribute(Type converterType)
        {
            ConverterType = converterType;
        }

        public Type ConverterType { get; }
    }
}