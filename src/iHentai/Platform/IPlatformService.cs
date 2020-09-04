using System.Threading.Tasks;

namespace iHentai.Platform
{
    public interface IPlatformService
    {
        string LocalPath { get; }
        Task<IFolderItem?> GetFolder(string token);
        Task<IFolderItem?> GetFolderFromPath(string path, string token);
        Task<IFolderItem?> GetFolderFromPath(string path);
    }
}
