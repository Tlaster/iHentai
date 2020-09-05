using Windows.Storage.Pickers;

namespace iHentai.Common.Extensions
{
#if !WINDOWS_UWP
    [ComImport]
    [Guid("3E68D4BD-7135-4D10-8018-9FB6D9F33FA1")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(true)]
    interface IInitializeWithWindow
    {
        void Initialize(
            IntPtr hwnd);
    }
#endif

    internal static class FolderPickerExtensions
    {
        public static void InitWindow(this FolderPicker picker)
        {
#if !WINDOWS_UWP
            ((IInitializeWithWindow)(object)picker).Initialize(System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle);
#endif
        }
    }
}