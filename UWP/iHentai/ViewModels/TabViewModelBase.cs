using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHentai.Common.Tab;

namespace iHentai.ViewModels
{
    class TabViewModelBase : ViewModelBase, ITabViewModel
    {
        public string Title { get; protected set; }
        public bool IsLoading { get; protected set; }
    }
}
