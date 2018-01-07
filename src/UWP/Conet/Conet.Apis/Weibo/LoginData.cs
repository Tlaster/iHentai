using Conet.Apis.Core.Models.Interfaces;
using iHentai.Services;
using PropertyChanged;

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