using System.IO;
using iHentai.Data.Models;
using iHentai.Extensions.Runtime;
using iHentai.Platform;
using LiteDB;

namespace iHentai.Data
{
    public class ExtensionDb : IExtensionStorage
    {
        public static ExtensionDb Instance { get; } = new ExtensionDb();
        private string DbFile => Path.Combine(this.Resolve<IPlatformService>().LocalPath, "extension.db");

        public void Set(string extensionKey, string key, string value)
        {
            using var db = new LiteDatabase(DbFile);
            var column = db.GetCollection<ExtensionStorageModel>();
            if (column.Exists(it => it.Key == key && it.ExtensionKey == extensionKey))
            {
                var item = column.FindOne(it => it.Key == key && it.ExtensionKey == extensionKey);
                item.Value = value;
                column.Update(item);
            }
            else
            {
                column.Insert(new ExtensionStorageModel
                {
                    ExtensionKey = extensionKey,
                    Key = key,
                    Value = value,
                });
            }
        }

        public string? Get(string extensionKey, string key)
        {
            using var db = new LiteDatabase(DbFile);
            var column = db.GetCollection<ExtensionStorageModel>();
            if (column.Exists(it => it.Key == key && it.ExtensionKey == extensionKey))
            {
                var item = column.FindOne(it => it.Key == key && it.ExtensionKey == extensionKey);
                return item.Value;
            }
            else
            {
                return null;
            }
        }
    }
}