using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using iHentai.Annotations;
using iHentai.Common.Collection;
using iHentai.Services.Manhuagui;
using iHentai.Services.Manhuagui.Model;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Helpers;

namespace iHentai.ViewModels.Manhuagui
{
    class ManhuaguiUpdateViewModel : TabViewModelBase
    {
        public List<ManhuaguiGallery> Source { get; private set; }

        public ManhuaguiUpdateViewModel()
        {
            Init();
        }

        private async void Init()
        {
            await Refresh();
        }


        public async Task Refresh()
        {
            if (IsLoading)
            {
                return;
            }
            IsLoading = true;
            Source = await Singleton<ManhuaguiApi>.Instance.Update();
            IsLoading = false;
        }
    }
}
