using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHentai.Platform;

namespace iHentai.Data
{
    class ScriptHistoryDb
    {
        public static ScriptHistoryDb Instance { get; } = new ScriptHistoryDb();
        private string DbFile => Path.Combine(this.Resolve<IPlatformService>().LocalPath, "library.db");
        private ScriptHistoryDb()
        {

        }


    }
}
