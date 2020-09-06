using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHentai.ViewModels;

namespace iHentai.Services.Models.Script
{
    class ScriptLoginViewModel : ViewModelBase
    {
        public ScriptLoginViewModel(ScriptApi api)
        {
            Api = api;
        }

        public ScriptApi Api { get; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsLoading { get; set; }

        public async Task<int> Login()
        {
            IsLoading = true;
            var result = await Api.Login(UserName, Password);
            IsLoading = false;
            return result;
        }
    }
}
