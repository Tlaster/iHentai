using iHentai.Services.Models.Core;
using LiteDB;

namespace iHentai.Data.Models
{
    public class LocalGalleryModel : IGallery
    {
        [BsonId]
        public long Id {get;set;}
        public string Name { get; set; }
        public string Path { get; set; }
        public string Token { get; set; }
        public long LibraryId { get; set; }
        [BsonIgnore]
        public string? Title => Name;
        public string? Thumb { get; set; }
    }
}
