using System;

namespace iHentai.Apis.Core.Models.Interfaces
{
    public enum ContentTypes
    {
        Info,
        DetailInfo,
        Detail
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    internal sealed class ContentTypeAttribute : Attribute
    {
        public ContentTypeAttribute(ContentTypes contentType)
        {
            ContentType = contentType;
        }

        public ContentTypes ContentType { get; }
    }

    public interface IGalleryContentView<T>
    {
    }
}