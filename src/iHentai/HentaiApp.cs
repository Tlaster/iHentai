using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using iHentai.Common;
using iHentai.Data;
using iHentai.Extensions;
using iHentai.Extensions.Runtime;
using iHentai.Platform;
using SevenZip;

namespace iHentai
{
    internal static class Ioc
    {
        private static readonly ConcurrentDictionary<Type, object>
            _container = new ConcurrentDictionary<Type, object>();

        public static void Register<T, V>(this object _, Func<V> generator) where V : T
        {
            _container.TryAdd(typeof(T), generator.Invoke());
        }

        public static void Register<T, V>(this object _) where V : T, new()
        {
            _container.TryAdd(typeof(T), new V());
        }

        public static T Resolve<T>(this object _)
        {
            if (_container.TryGetValue(typeof(T), out var result))
            {
                return (T) result;
            }

            return default;
        }
    }

    internal class HentaiApp
    {
        public static HentaiApp Instance { get; } = new HentaiApp();

        public void Init()
        {
            this.Register<HttpMessageHandler, HentaiHttpHandler>(() => HentaiHttpHandler.Instance);
            this.Register<IExtensionStorage, ExtensionStorage>();
            this.Register<IPlatformService, PlatformService>();
            if (SettingsManager.Instance.EnableExtension)
            {
                if (SettingsManager.Instance.UseLocalExtension)
                {
                    this.Register<IExtensionManager, LocalExtensionManager>();
                }
                else
                {
                    this.Register<IExtensionManager, NetworkExtensionManager>();
                }
                this.Resolve<IExtensionManager>().Init();
            }
            var sevenZipPath = (RuntimeInformation.ProcessArchitecture) switch
            {
                Architecture.X86 => Path.Combine(Environment.CurrentDirectory, "Assets", "7z", "x86", "7zUWP.dll"),
                Architecture.X64 => Path.Combine(Environment.CurrentDirectory, "Assets", "7z", "x64", "7zUWP.dll"),
                Architecture.Arm => Path.Combine(Environment.CurrentDirectory, "Assets", "7z", "arm", "7zUWP.dll"),
                Architecture.Arm64 => Path.Combine(Environment.CurrentDirectory, "Assets", "7z", "arm64", "7zUWP.dll"),
                _ => throw new ArgumentOutOfRangeException()
            };
            SevenZipBase.SetLibraryPath(sevenZipPath);
        }
    }
}