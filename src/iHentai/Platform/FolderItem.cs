using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using static iHentai.Common.Helpers.NativeFindStorageItemHelper;
using FileAttributes = System.IO.FileAttributes;

namespace iHentai.Platform
{
    internal class PathFileItem : IFileItem
    {
        public PathFileItem(string path, string token, DateTime creationTime)
        {
            Path = path;
            Token = token;
            CreationTime = creationTime;
        }

        public string Path { get; }
        public string Extension => System.IO.Path.GetExtension(Path);

        public async Task<string> ReadAllTextAsync()
        {
            var file = await StorageFile.GetFileFromPathAsync(Path);
            return await FileIO.ReadTextAsync(file);
        }

        public string Name => System.IO.Path.GetFileName(Path);
        public string Token { get; }
        public DateTime CreationTime { get; }
    }

    internal class PathFolderItem : IFolderItem
    {
        public PathFolderItem(string path, string token, DateTime creationTime)
        {
            Path = path;
            Token = token;
            CreationTime = creationTime;
        }

        public string Path { get; }
        public string Name => System.IO.Path.GetFileNameWithoutExtension(Path);
        public string Token { get; }
        public DateTime CreationTime { get; }

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
                            FileAttributes.System &&
                            ((FileAttributes) findData.dwFileAttributes & FileAttributes.Directory) !=
                            FileAttributes.Directory)
                        {
                            FileTimeToSystemTime(ref findData.ftCreationTime, out var systemCreatedDateOutput);
                            var itemCreatedDate = new DateTime(
                                systemCreatedDateOutput.Year, systemCreatedDateOutput.Month,
                                systemCreatedDateOutput.Day,
                                systemCreatedDateOutput.Hour, systemCreatedDateOutput.Minute,
                                systemCreatedDateOutput.Second, systemCreatedDateOutput.Milliseconds,
                                DateTimeKind.Utc);
                            result.Add(new PathFileItem(System.IO.Path.Combine(Path, findData.cFileName), Token,
                                itemCreatedDate));
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
                            FileAttributes.System &&
                            ((FileAttributes) findData.dwFileAttributes & FileAttributes.Directory) ==
                            FileAttributes.Directory && findData.cFileName != "." && findData.cFileName != "..")
                        {
                            FileTimeToSystemTime(ref findData.ftCreationTime, out var systemCreatedDateOutput);
                            var itemCreatedDate = new DateTime(
                                systemCreatedDateOutput.Year, systemCreatedDateOutput.Month,
                                systemCreatedDateOutput.Day,
                                systemCreatedDateOutput.Hour, systemCreatedDateOutput.Minute,
                                systemCreatedDateOutput.Second, systemCreatedDateOutput.Milliseconds,
                                DateTimeKind.Utc);
                            result.Add(new PathFolderItem(System.IO.Path.Combine(Path, findData.cFileName),
                                Token, itemCreatedDate));
                        }
                    } while (FindNextFile(hFile, out findData));

                    FindClose(hFile);
                }

                return result as IEnumerable<IFolderItem>;
            });
        }
    }

    internal class FileItem : IFileItem
    {
        private readonly StorageFile _file;

        public FileItem(StorageFile file, string token)
        {
            _file = file;
            Token = token;
        }

        public async Task<string> ReadAllTextAsync()
        {
            var file = await StorageFile.GetFileFromPathAsync(Path);
            return await FileIO.ReadTextAsync(file);
        }

        public string Extension => System.IO.Path.GetExtension(_file.Path);

        public string Name => _file.Name;

        public string Path => _file.Path;

        public string Token { get; }
        public DateTime CreationTime => _file.DateCreated.DateTime;
    }

    internal class FolderItem : IFolderItem
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
        public DateTime CreationTime => _folder.DateCreated.DateTime;

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