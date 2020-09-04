using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHentai.Extensions;
using iHentai.Extensions.Models;

namespace iHentai.ViewModels.Extensions
{
    class ExtensionsListViewModel : ViewModelBase
    {
        public ObservableCollection<ExtensionManifest> Source => this.Resolve<IExtensionManager>().Extensions;
    }
}
