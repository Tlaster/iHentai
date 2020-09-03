using LiteDB;

namespace iHentai.Data.Models
{
    public class LocalLibraryModel
    {
        [BsonId]
        public long Id {get;set;}

        public string Path { get; set; }

        public string Token { get; set; }

        public int Count { get; set; }
    }
}
