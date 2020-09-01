using System.Collections.Generic;
using System.IO;
using System.Linq;
using iHentai.Data.Models;
using iHentai.Platform;
using LiteDB;

namespace iHentai.Data
{
    public class LocalLibraryDb
    {
        public static LocalLibraryDb Instance { get; } = new LocalLibraryDb();
        private string DbFile => Path.Combine(HentaiApp.Instance.Resolve<IPlatformService>().LocalPath, "library.db");
        private LocalLibraryDb()
        {
            
        }

        public List<LocalLibraryModel> GetLocalLibrary()
        {
            return GetAll<LocalLibraryModel>();
        }

        public LocalLibraryModel AddLocalLibrary(IFolderItem fodler, List<LocalGalleryModel> gallery)
        {
            using var db = new LiteDatabase(DbFile);
            var column = db.GetCollection<LocalLibraryModel>();
            if (column.Exists(it => it.Token == fodler.Token))
            {
                return column.FindOne(it => it.Token == fodler.Token);
            }

            var id = column.Insert(new LocalLibraryModel
            {
                Path = fodler.Path,
                Token = fodler.Token,
            });
            gallery.ForEach(model =>
            {
                model.LibraryId = id;
                var column = db.GetCollection<LocalGalleryModel>();
                if (column.Exists(it => it.Path == model.Path))
                {
                    return;
                }
                column.Insert(model);
            });
            return column.FindById(id);
        }

        public void UpdateLocalLibrary(LocalLibraryModel model, List<LocalGalleryModel> gallery)
        {
            using var db = new LiteDatabase(DbFile);
            var galleryColumn = db.GetCollection<LocalGalleryModel>();
            galleryColumn.DeleteMany(it => model.Id == it.LibraryId);
            gallery.ForEach(it => it.LibraryId = model.Id);
            galleryColumn.InsertBulk(gallery);
        }

        public void RemoveLocalLibrary(LocalLibraryModel model)
        {
            Remove<LocalLibraryModel>(model.Id);
            using var db = new LiteDatabase(DbFile);
            var galleryColumn = db.GetCollection<LocalGalleryModel>();
            galleryColumn.DeleteMany(it => it.LibraryId == model.Id);
        }

        public List<LocalGalleryModel> GetLocalGallery()
        {
            return GetAll<LocalGalleryModel>();
        }

        public void AddLocalGallery(LocalGalleryModel model)
        {
            using var db = new LiteDatabase(DbFile);
            var column = db.GetCollection<LocalGalleryModel>();
            if (column.Exists(it => it.Path == model.Path))
            {
                return;
            }
            column.Insert(model);
        }

        public void RemoveLocalGallery(LocalGalleryModel model)
        {
            Remove<LocalGalleryModel>(model.Id);
        }


        private List<T> GetAll<T>()
        {
            using var db = new LiteDatabase(DbFile);
            var column = db.GetCollection<T>();
            return column.FindAll().ToList();
        }

        private void Remove<T>(BsonValue id)
        {
            using var db = new LiteDatabase(DbFile);
            var column = db.GetCollection<T>();
            column.Delete(id);
        }
    }
}
