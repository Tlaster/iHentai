namespace iHentai.Services.Core
{
    interface IMangaGallery : IGallery
    {
        string UpdateAt { get; }
        string Chapter { get; }
    }

    interface IMangaDetail : IGallery
    {
        
    }
}