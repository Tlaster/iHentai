using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace iHentai.Paging
{
    internal static class Keyboard
    {
        public static bool IsControlKeyDown => IsKeyDown(VirtualKey.Control);

        public static bool IsShiftKeyDown => IsKeyDown(VirtualKey.Shift);

        public static bool IsAltKeyDown => IsKeyDown(VirtualKey.LeftMenu);

        private static bool IsKeyDown(VirtualKey key)
        {
            return (Window.Current.CoreWindow.GetKeyState(key) & CoreVirtualKeyStates.Down) ==
                   CoreVirtualKeyStates.Down;
        }
    }
}