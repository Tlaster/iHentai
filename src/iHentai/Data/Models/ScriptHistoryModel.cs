using LiteDB;

namespace iHentai.Data.Models
{
    internal class ScriptHistoryModel
    {
        [BsonId] public long Id { get; set; }

        public string ExtensionId { get; set; }
        public string GalleryId { get; set; }
        public string ChapterId { get; set; }
    }
}