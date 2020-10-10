using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHentai.Common;
using iHentai.Data.Models;
using iHentai.Platform;
using iHentai.Services.Models.Core;
using LiteDB;

namespace iHentai.Data
{
    class ReadingHistoryDb
    {
        public static ReadingHistoryDb Instance { get; } = new ReadingHistoryDb();
        private string DbFile => Path.Combine(this.Resolve<IPlatformService>().LocalPath, "history.db");
        public ObservableCollection<ReadingHistoryModel> Source { get; }

        public ReadingHistoryDb()
        {
            Source = new ObservableCollection<ReadingHistoryModel>(GetAll());
        }

        public ReadingHistoryModel AddOrUpdate(string title, string thumb, string galleryId, GalleryType type, string extra, string extraType)
        {
            using var db = new LiteDatabase(new ConnectionString
            {
                Filename = DbFile,
                Connection = ConnectionType.Shared,
            });
            var column = db.GetCollection<ReadingHistoryModel>();
            var current = column.FindOne(it => it.GalleryId == galleryId);
            if (current != null)
            {
                Update(current);
                return current;
            }
            else
            {
                var item = new ReadingHistoryModel
                {
                    GalleryId = galleryId,
                    GalleryType = type,
                    Extra = extra,
                    ExtraType = extraType,
                    ReadAt = DateTime.UtcNow,
                    Title = title,
                    Thumb = thumb,
                };

                var id = column.Insert(item);
                var result = column.FindById(id);
                Source.Insert(0, result);
                return result;
            }
        }

        public void Update(ReadingHistoryModel model)
        {
            var index = Source.IndexOf(model);
            model.ReadAt = DateTime.UtcNow;
            using var db = new LiteDatabase(new ConnectionString
            {
                Filename = DbFile,
                Connection = ConnectionType.Shared,
            });
            var column = db.GetCollection<ReadingHistoryModel>();
            column.Update(model);
            Source.Move(index, 0);
        }

        private List<ReadingHistoryModel> GetAll()
        {
            //actual not work
            using var db = new LiteDatabase(new ConnectionString
            {
                Filename = DbFile,
                Connection = ConnectionType.Shared,
            });
            var column = db.GetCollection<ReadingHistoryModel>();
            return column.FindAll().OrderByDescending(it => it.ReadAt).ToList();
        }
    }
}
