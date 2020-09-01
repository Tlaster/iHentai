using LiteDB;

namespace iHentai.Data.Models
{
    public class ExtensionStorageModel
    {
        [BsonId]
        public long Id {get;set;}
        public string ExtensionKey { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}