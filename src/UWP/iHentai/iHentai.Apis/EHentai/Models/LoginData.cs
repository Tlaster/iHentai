using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHentai.Services;

namespace iHentai.Apis.EHentai.Models
{
    public class LoginData : ILoginData
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
