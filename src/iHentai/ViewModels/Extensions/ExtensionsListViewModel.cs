using System.Collections.ObjectModel;
using iHentai.Extensions;
using iHentai.Extensions.Models;

namespace iHentai.ViewModels.Extensions
{
    internal class ExtensionsListViewModel : ViewModelBase
    {
        public ObservableCollection<ExtensionManifest> Source => this.Resolve<IExtensionManager>().Extensions;
    }
}