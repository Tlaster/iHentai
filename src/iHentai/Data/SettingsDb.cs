using System.IO;
using iHentai.Data.Models;
using iHentai.Platform;
using LiteDB;

namespace iHentai.Data
{
    public class SettingsDb
    {
        public static SettingsDb Instance { get; } = new SettingsDb();
        private string DbFile => Path.Combine(this.Resolve<IPlatformService>().LocalPath, "settings.db");

        public void Set(string key, string value)
        {
            using var db = new LiteDatabase(DbFile);
            var column = db.GetCollection<SettingItemModel>();
            if (column.Exists(it => it.Key == key))
            {
                var item = column.FindOne(it => it.Key == key);
                item.Value = value;
                column.Update(item);
            }
            else
            {
                column.Insert(new SettingItemModel
                {
                    Key = key,
                    Value = value,
                });
            }
        }

        public string? Get(string key, string? defaultValue)
        {
            using var db = new LiteDatabase(DbFile);
            var column = db.GetCollection<SettingItemModel>();
            if (column.Exists(it => it.Key == key))
            {
                var item = column.FindOne(it => it.Key == key);
                return item.Value;
            }
            else
            {
                return defaultValue;
            }
        }

    }
}
