using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace iHentai.Data.Models
{
    class ScriptHistoryModel
    {
        [BsonId]
        public long Id { get; set; }

        public string ExtensionId { get; set; }
        public string GalleryId { get; set; }
        public string ChapterId { get; set; }
    }
}
