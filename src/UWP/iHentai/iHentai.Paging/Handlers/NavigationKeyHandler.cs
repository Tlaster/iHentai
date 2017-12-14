using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation.Metadata;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace iHentai.Paging.Handlers
{
    internal class NavigationKeyHandler
    {
        private static BackKeyPressedHandler _handler;
        private readonly List<Func<CancelEventArgs, Task>> _goBackActions;

        private readonly HentaiPage _page;

        public NavigationKeyHandler(HentaiPage page)
        {
            _page = page;
            _goBackActions = new List<Func<CancelEventArgs, Task>>();

            if (DesignMode.DesignModeEnabled)
                return;

            page.Loaded += OnPageLoaded;
            page.Unloaded += OnPageUnloaded;
        }

        public Func<CancelEventArgs, Task> AddGoBackHandler(Action<CancelEventArgs> action)
        {
            var func = new Func<CancelEventArgs, Task>(args =>
            {
                action(args);
                return null;
            });

            AddGoBackAsyncHandler(func);
            return func;
        }

        public void AddGoBackAsyncHandler(Func<CancelEventArgs, Task> action)
        {
            _goBackActions.Insert(0, action);
        }

        public void RemoveGoBackAsyncHandler(Func<CancelEventArgs, Task> action)
        {
            _goBackActions.Remove(action);
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            var page = (HentaiPage) sender;

            if (_handler == null)
                _handler = new BackKeyPressedHandler();

            _handler.Add(page, s => TryGoBackAsync());
            Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated += OnAcceleratorKeyActivated;
            Window.Current.CoreWindow.PointerPressed += OnPointerPressed;
        }

        private void OnPageUnloaded(object sender, RoutedEventArgs e)
        {
            var page = (HentaiPage) sender;
            if (ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                _handler.Remove(page);
            }
            else
            {
                Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated -= OnAcceleratorKeyActivated;
                Window.Current.CoreWindow.PointerPressed -= OnPointerPressed;
            }
        }

        private void OnAcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs args)
        {
            _page.OnKeyActivated(args);

            if (args.KeyStatus.IsKeyReleased)
                _page.OnKeyUp(args);

            if (args.Handled)
                return;

            var virtualKey = args.VirtualKey;
            if (args.EventType == CoreAcceleratorKeyEventType.KeyDown ||
                args.EventType == CoreAcceleratorKeyEventType.SystemKeyDown)
            {
                var isLeftOrRightKey =
                    _page.UseAltLeftOrRightToNavigate && (
                        virtualKey == VirtualKey.Left ||
                        virtualKey == VirtualKey.Right ||
                        (int) virtualKey == 166 ||
                        (int) virtualKey == 167);

                var isBackKey = _page.UseBackKeyToNavigate && virtualKey == VirtualKey.Back;

                if (isLeftOrRightKey || isBackKey)
                {
                    if (PopupHelper.IsPopupVisible)
                        return;

                    var element = FocusManager.GetFocusedElement();
                    if (element is FrameworkElement && PopupHelper.IsInPopup((FrameworkElement) element))
                        return;

                    if (isBackKey)
                    {
                        if (!(element is TextBox) && !(element is PasswordBox) &&
                            (_page.UseBackKeyToNavigateInWebView || !(element is WebView)) && _page.Frame.CanGoBack)
                        {
                            args.Handled = true;
                            TryGoBackAsync();
                        }
                    }
                    else
                    {
                        var altKey = Keyboard.IsAltKeyDown;
                        var controlKey = Keyboard.IsControlKeyDown;
                        var shiftKey = Keyboard.IsShiftKeyDown;

                        var noModifiers = !altKey && !controlKey && !shiftKey;
                        var onlyAlt = altKey && !controlKey && !shiftKey;

                        if ((int) virtualKey == 166 && noModifiers || virtualKey == VirtualKey.Left && onlyAlt)
                        {
                            args.Handled = true;
                            TryGoBackAsync();
                        }
                        else if ((int) virtualKey == 167 && noModifiers || virtualKey == VirtualKey.Right && onlyAlt)
                        {
                            args.Handled = true;
                            TryGoForwardAsync();
                        }
                    }
                }
            }
        }

        private void OnPointerPressed(CoreWindow sender, PointerEventArgs args)
        {
            if (!_page.UsePointerButtonsToNavigate)
                return;

            var properties = args.CurrentPoint.Properties;
            if (properties.IsLeftButtonPressed || properties.IsRightButtonPressed || properties.IsMiddleButtonPressed)
                return;

            var backPressed = properties.IsXButton1Pressed;
            var forwardPressed = properties.IsXButton2Pressed;
            if (backPressed ^ forwardPressed)
            {
                args.Handled = true;

                if (backPressed)
                    TryGoBackAsync();

                if (forwardPressed)
                    TryGoForwardAsync();
            }
        }

        private async void TryGoForwardAsync()
        {
            if (_page.Frame.CanGoForward)
                await _page.Frame.GoForwardAsync();
        }

        private bool TryGoBackAsync()
        {
            if (_page.Frame.CanGoBack)
            {
                var args = new CancelEventArgs();
                CallGoBackActions(args, _goBackActions);
                return _page.Frame.CanGoBack || args.Cancel;
            }
            return false;
        }

        private void CallGoBackActions(CancelEventArgs e, List<Func<CancelEventArgs, Task>> actions)
        {
            Func<CancelEventArgs, Task> lastAction = null;
            var copy = new CancelEventArgs();
            foreach (var action in actions)
            {
                lastAction = action;

                var task = action(copy);
                if (task != null && !task.IsCompleted)
                {
                    e.Cancel = true;
                    task.ContinueWith(t => { CheckGoBackActions(actions, action, copy, true); });
                    return;
                }

                if (copy.Cancel)
                    break;
            }

            e.Cancel = copy.Cancel;
            CheckGoBackActions(actions, lastAction, copy, false);
        }

        private void CheckGoBackActions(List<Func<CancelEventArgs, Task>> actions, Func<CancelEventArgs, Task> action,
            CancelEventArgs copy, bool perform)
        {
            if (!copy.Cancel)
            {
                var nextActions = actions.Skip(actions.IndexOf(action) + 1).ToList();
                if (nextActions.Count == 0)
                {
                    GoBack();
                }
                else
                {
                    CallGoBackActions(copy, nextActions);
                    if (!copy.Cancel)
                        GoBack();
                }
            }
        }

        private void GoBack()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _page.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async delegate
            {
                if (_page.Frame.CanGoBack && !_page.Frame.IsNavigating)
                    await _page.Frame.GoBackAsync();
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }
    }
}