using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHentai.Services;

namespace Conet.Mastodon.Models
{
    public class InstanceData : IInstanceData
    {
        public string AccessToken { get; set; }
        public long Uid { get; set; }
        public string Domain { get; set; }
    }
}
