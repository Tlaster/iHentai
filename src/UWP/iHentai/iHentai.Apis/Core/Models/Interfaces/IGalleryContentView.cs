using System;

namespace iHentai.Apis.Core.Models.Interfaces
{
    public enum ContentTypes
    {
        Info,
        DetailInfo,
        Detail,
        Search
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ContentTypeAttribute : Attribute
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