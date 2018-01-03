using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHentai.Services;

namespace iHentai.Apis.EHentai.Models
{
    public class InstanceData : IInstanceData
    {
        public Dictionary<string, string> Cookies { get; set; }

        public Config ApiConfig { get; set; } = new Config();
    }
}
