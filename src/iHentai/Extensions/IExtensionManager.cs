using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using iHentai.Extensions.Models;
using iHentai.Services;

namespace iHentai.Extensions
{
    public interface IExtensionManager
    {
        ObservableCollection<ExtensionManifest> Extensions { get; }
        Task<ScriptApi?> GetApi(ExtensionManifest manifest);
        Task Init();
        Task Reload();
        Task<ScriptApi?> GetApiById(string extensionId);
    }

    public interface INetworkExtensionManager: IExtensionManager
    { 
        ObservableCollection<NetworkExtensionModel> NetworkExtension { get; }
        Task<ExtensionManifest> InstallExtension(NetworkExtensionModel model);
        Task UnInstallExtension(ExtensionManifest model);
    }
}