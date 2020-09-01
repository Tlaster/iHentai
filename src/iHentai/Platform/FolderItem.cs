using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace iHentai.Platform
{
    class FileItem : IFileItem
    {
        private readonly StorageFile _file;

        public FileItem(StorageFile file, string token)
        {
            _file = file;
            Token = token;
        }

        public string Extension => System.IO.Path.GetExtension(_file.Path);

        public string Name => _file.Name;

        public string Path => _file.Path;

        public string Token { get; }
    }

    class FolderItem : IFolderItem
    {
        private readonly StorageFolder _folder;

        public FolderItem(StorageFolder folder, string token)
        {
            _folder = folder;
            Token = token;
        }

        public string Name => _folder.Name;

        public string Path => _folder.Path;

        public string Token { get; }

        public async Task<IEnumerable<IFileItem>> GetFiles()
        {
            var files = await _folder.GetFilesAsync();
            return files.Select(it => new FileItem(it, Token));
        }

        public async Task<IEnumerable<IFolderItem>> GetFolders()
        {
            var folders = await _folder.GetFoldersAsync();
            return folders.Select(it => new FolderItem(it, Token));
        }
    }
}
