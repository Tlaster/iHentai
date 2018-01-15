using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHentai.Services;

namespace Conet.Apis.Weibo
{
    [Equals]
    public class InstanceData : IInstanceData
    {
        public string AccessToken { get; set; }
        public long Uid { get; set; }
        public string Source { get; set; }
    }
}
