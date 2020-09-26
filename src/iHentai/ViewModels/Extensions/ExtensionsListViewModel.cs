using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.AllJoyn;
using iHentai.Extensions;
using iHentai.Extensions.Models;
using Microsoft.Toolkit.Uwp.UI;

namespace iHentai.ViewModels.Extensions
{
    internal class ExtensionsListViewModel : ViewModelBase
    {
        private readonly INetworkExtensionManager _manager;

        public static ExtensionsListViewModel Instance { get; } = new ExtensionsListViewModel();

        private ExtensionsListViewModel()
        {
            if (!(this.Resolve<IExtensionManager>() is INetworkExtensionManager manager))
            {
                throw new ArgumentException("Require network extension manager");
            }

            _manager = manager;
        }

        public AdvancedCollectionView NetworkExtensions => new AdvancedCollectionView(_manager.NetworkExtension)
        {
            Filter = o => _manager.Extensions.All(it => it.Name != (o as NetworkExtensionModel).Name)
        };

        public ObservableCollection<ExtensionManifest> InstalledExtensions => _manager.Extensions;
        
        public async Task InstallExtension(NetworkExtensionModel model)
        {
            await _manager.InstallExtension(model);
            NetworkExtensions.Refresh();
        }

        public async Task UnInstallExtension(ExtensionManifest model)
        {
            await _manager.UnInstallExtension(model);
            NetworkExtensions.Refresh();
        }
    }
}