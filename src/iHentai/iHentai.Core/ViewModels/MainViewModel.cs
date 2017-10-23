using System;
using System.Collections.Generic;
using System.Text;
using iHentai.Services.Core;
using PropertyChanged;

namespace iHentai.Core.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MainViewModel
    {
        
        private readonly IHentaiApis _apis;
        
        public MainViewModel(IHentaiApis apis)
        {
            _apis = apis;
        }
    }
}
