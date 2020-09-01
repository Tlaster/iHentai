using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using iHentai.Common.Helpers;
using static iHentai.Common.Helpers.NativeFindStorageItemHelper;
using FileAttributes = System.IO.FileAttributes;

namespace iHentai.Platform
{

    class PathFileItem : IFileItem
    {
        public PathFileItem(string path, string token)
        {
            Path = path;
            Token = token;
        }

        public string Path { get; }
        public string Extension => System.IO.Path.GetExtension(Path);
        public string Name => System.IO.Path.GetFileName(Path);
        public string Token { get; }
    }

    class PathFolderItem : IFolderItem
    {
        public PathFolderItem(string path, string token)
        {
            Path = path;
            Token = token;
        }
        public string Path { get; }
        public string Name => System.IO.Path.GetFileNameWithoutExtension(Path);
        public string Token { get; }

        public Task<IEnumerable<IFileItem>> GetFiles()
        {
            return Task.Run(() =>
            {
                var result = new List<IFileItem>();
                var findInfoLevel = FINDEX_INFO_LEVELS.FindExInfoBasic;
                var additionalFlags = FIND_FIRST_EX_LARGE_FETCH;

                var hFile = FindFirstFileExFromApp(Path + "\\*.*", findInfoLevel, out var findData,
                    FINDEX_SEARCH_OPS.FindExSearchNameMatch, IntPtr.Zero,
                    additionalFlags);

                if (hFile.ToInt64() != -1)
                {
                    do
                    {
                        if (((FileAttributes) findData.dwFileAttributes & FileAttributes.Hidden) !=
                            FileAttributes.Hidden &&
                            ((FileAttributes) findData.dwFileAttributes & FileAttributes.System) !=
                            FileAttributes.System && ((FileAttributes) findData.dwFileAttributes & FileAttributes.Directory) !=
                            FileAttributes.Directory)
                        {
                            result.Add(new PathFileItem(System.IO.Path.Combine(Path, findData.cFileName), Token));
                        }
                    } while (FindNextFile(hFile, out findData));

                    FindClose(hFile);
                }

                return result as IEnumerable<IFileItem>;
            });
        }

        public Task<IEnumerable<IFolderItem>> GetFolders()
        {
            return Task.Run(() =>
            {
                var result = new List<IFolderItem>();
                var findInfoLevel = FINDEX_INFO_LEVELS.FindExInfoBasic;
                var additionalFlags = FIND_FIRST_EX_LARGE_FETCH;

                var hFile = FindFirstFileExFromApp(Path + "\\*.*", findInfoLevel, out var findData,
                    FINDEX_SEARCH_OPS.FindExSearchNameMatch, IntPtr.Zero,
                    additionalFlags);

                if (hFile.ToInt64() != -1)
                {
                    do
                    {
                        if (((FileAttributes) findData.dwFileAttributes & FileAttributes.Hidden) !=
                            FileAttributes.Hidden &&
                            ((FileAttributes) findData.dwFileAttributes & FileAttributes.System) !=
                            FileAttributes.System && ((FileAttributes) findData.dwFileAttributes & FileAttributes.Directory) ==
                            FileAttributes.Directory && findData.cFileName != "." && findData.cFileName != "..")
                        {
                            result.Add(new PathFolderItem(System.IO.Path.Combine(Path, findData.cFileName),
                                Token));
                        }
                    } while (FindNextFile(hFile, out findData));

                    FindClose(hFile);
                }

                return result as IEnumerable<IFolderItem>;
            });
        }
    }

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
