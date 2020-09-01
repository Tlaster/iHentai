using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using iHentai.Data;
using iHentai.Data.Models;
using iHentai.Platform;
using MimeTypes;

namespace iHentai.Services
{
    public class LocalLibraryManager
    {
        private LocalLibraryManager()
        {
        }


        public ObservableCollection<LocalLibraryModel> LocalLibrary { get; } =
            new ObservableCollection<LocalLibraryModel>(LocalLibraryDb.Instance.GetLocalLibrary());

        public ObservableCollection<LocalGalleryModel> LocalGallery { get; } =
            new ObservableCollection<LocalGalleryModel>(LocalLibraryDb.Instance.GetLocalGallery());

        public static LocalLibraryManager Instance { get; } = new LocalLibraryManager();


        public async Task Refresh()
        {
            foreach (var item in LocalLibrary)
            {
                var gallery = await ScanForGalleryAsync(await HentaiApp.Instance.Resolve<IPlatformService>().GetFolder(item.Token));
                LocalLibraryDb.Instance.UpdateLocalLibrary(item, gallery);
            }
            LocalGallery.Clear();
            LocalLibraryDb.Instance.GetLocalGallery().ForEach(it => LocalGallery.Add(it));
        }



        public async Task AddFolder(params IFolderItem[] paths)
        {
            foreach (var path in paths)
            {
                var gallery = await ScanForGalleryAsync(path);
                var result = LocalLibraryDb.Instance.AddLocalLibrary(path, gallery);
                LocalLibrary.Add(result);
                gallery.ForEach(it => LocalGallery.Add(it));
            }
        }

        public void RemoveFolder(LocalLibraryModel model)
        {
            LocalLibraryDb.Instance.RemoveLocalLibrary(model);
            LocalLibrary.Remove(model);
            LocalGallery.Where(it => it.LibraryId == model.Id).ToList().ForEach(it => LocalGallery.Remove(it));
        }


        public async Task<List<string>> GetGalleryImages(LocalGalleryModel item)
        {
            var folder = await HentaiApp.Instance.Resolve<IPlatformService>().GetFolderFromPath(item.Path, item.Token);
            var files = await folder.GetFiles();
            return files.Where(it => MimeTypeMap.GetMimeType(it.Extension).StartsWith("image"))
                .Select(it => it.Path).ToList();
        }

        private async Task<List<LocalGalleryModel>> ScanForGalleryAsync(IFolderItem folder)
        {
            var result = new List<LocalGalleryModel>();

            var files = await folder.GetFiles();
            var imageFileCount = files.Count(it => MimeTypeMap.GetMimeType(it.Extension).StartsWith("image"));
            if (Convert.ToDouble(imageFileCount) / Convert.ToDouble(files.Count()) > 0.9)
            {
                result.Add(new LocalGalleryModel
                {
                    Name = folder.Name,
                    Path = folder.Path,
                    Token = folder.Token,
                    CreationTime = folder.CreationTime,
                    TotalFiles = files.Count(it => MimeTypeMap.GetMimeType(it.Extension).StartsWith("image")),
                    ReadFiles = 0,
                    Thumb = files.FirstOrDefault(it => MimeTypeMap.GetMimeType(it.Extension).StartsWith("image"))
                        ?.Path
                });
            }
            var folders = await folder.GetFolders();

            foreach (var child in folders)
            {
                var childItems = await ScanForGalleryAsync(child);
                result.AddRange(childItems);
            }

            return result;
        }
    }
}