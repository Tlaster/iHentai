using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHentai.Services.EHentai;
using Microsoft.Toolkit.Helpers;

namespace iHentai.ViewModels.EHentai
{
    class LoginViewModel : TabViewModelBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public async Task Login()
        {
            IsLoading = true;
            await Singleton<ExApi>.Instance.Login(UserName, Password);
            IsLoading = false;
        }
    }
}
