using LiteDB;

namespace iHentai.Data.Models
{
    public class SettingItemModel
    {
        [BsonId]
        public long Id {get;set;}
        public string? Key { get; set; }
        public string? Value { get; set; }
    }
}
