using Conet.Apis.Core.Models.Interfaces;
using iHentai.Services;

namespace Conet.Apis.Mastodon
{
    public class LoginData : ILoginData
    {
        public string Domain { get; set; }
    }
}