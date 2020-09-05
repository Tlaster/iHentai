using System.IO;
using iHentai.Platform;

namespace iHentai.Data
{
    internal class ScriptHistoryDb
    {
        private ScriptHistoryDb()
        {
        }

        public static ScriptHistoryDb Instance { get; } = new ScriptHistoryDb();
        private string DbFile => Path.Combine(this.Resolve<IPlatformService>().LocalPath, "library.db");
    }
}