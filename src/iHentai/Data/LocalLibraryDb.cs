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
        private LocalLibraryDb()
        {
        }

        public static LocalLibraryDb Instance { get; } = new LocalLibraryDb();
        private string DbFile => Path.Combine(this.Resolve<IPlatformService>().LocalPath, "library.db");

        public List<LocalLibraryModel> GetLocalLibrary()
        {
            return GetAll<LocalLibraryModel>();
        }

        public LocalLibraryModel AddLocalLibrary(IFolderItem folder, List<LocalGalleryModel> gallery)
        {
            using var db = new LiteDatabase(DbFile);
            var column = db.GetCollection<LocalLibraryModel>();
            if (column.Exists(it => it.Token == folder.Token))
            {
                return column.FindOne(it => it.Token == folder.Token);
            }

            var id = column.Insert(new LocalLibraryModel
            {
                Path = folder.Path,
                Token = folder.Token,
                Count = gallery.Count
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
            var deleted = galleryColumn.DeleteMany(it => gallery.Select(g => g.Path).All(g => g != it.Path));
            gallery.ForEach(it => it.LibraryId = model.Id);
            var news = gallery.Where(it => galleryColumn.FindOne(g => g.Path == it.Path) == null);
            galleryColumn.InsertBulk(news);
            model.Count = gallery.Count;
            db.GetCollection<LocalLibraryModel>().Update(model);
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

        public LocalGalleryModel? FindGallery(string path)
        {
            using var db = new LiteDatabase(DbFile);
            var column = db.GetCollection<LocalGalleryModel>();
            return column.Exists(it => it.Path == path) ? column.FindOne(it => it.Path == path) : null;
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