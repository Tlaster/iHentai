using System;
using System.Collections.Generic;
using AngleSharp.Dom;
using iHentai.Common.Html;

namespace iHentai.Services.Core
{
    interface IMangaGallery : IGallery
    {
        string UpdateAt { get; }
        string Chapter { get; }
    }

    interface IMangaDetail : IGallery
    {
        string Desc { get; }

        IEnumerable<IMangaChapter> Chapters { get; }
    }

    interface IMangaChapter
    {
        string Title { get; }
        bool Updated { get; }
    }

    class NullToBoolHtmlConverter : IHtmlConverter
    {
        public object ReadHtml(INode? node, Type targetType, object? existingValue)
        {
            if (targetType == typeof(bool))
            {
                return node != null;
            }

            return null;
        }
    }
}