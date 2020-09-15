using System;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;

namespace iHentai
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var activatedArgs = AppInstance.GetActivatedEventArgs();
            if (AppInstance.RecommendedInstance != null)
            {
                AppInstance.RecommendedInstance.RedirectActivationTo();
            }
            else
            {
                var key = Guid.NewGuid().ToString();
                if (activatedArgs is FileActivatedEventArgs fileActivatedEventArgs &&
                    fileActivatedEventArgs.Files.FirstOrDefault() is StorageFile file)
                {
                    key = Path.Combine(file.Path, file.Name);
                }

                var instance = AppInstance.FindOrRegisterInstanceForKey(key);
                if (instance.IsCurrentInstance)
                {
                    Application.Start(p => new App());
                }
                else
                {
                    instance.RedirectActivationTo();
                }
            }
        }
    }
}