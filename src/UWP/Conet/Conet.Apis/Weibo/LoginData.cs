using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conet.Apis.Core.Models.Interfaces;

namespace Conet.Apis.Weibo
{
    public class LoginData : ILoginData
    {
        public string AppID { get; set; }
        public string AppSecret { get; set; }
        public string RedirectUri { get; set; }
        public string Scope { get; set; }
        public string PackageName { get; set; }
    }
}
