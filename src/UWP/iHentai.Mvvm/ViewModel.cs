using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using iHentai.Basic.Helpers;
using iHentai.Paging;

namespace iHentai.Mvvm
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        static ViewModel()
        {
            KnownViews = Application.Current.GetType().GetTypeInfo().Assembly.DefinedTypes
                .Select(item =>
                    item.IsClass &&
                    ReflectionHelper.ImplementsGenericDefinition(item, typeof(IMvvmView<>), out var res)
                        ? new {ViewType = item, GenericType = res.GetGenericArguments().FirstOrDefault()}
                        : null).Where(item => item != null)
                .ToDictionary(item => item.GenericType, item => item.ViewType);
        }

        protected internal HentaiFrame Frame { get; internal set; }

        public static Dictionary<Type, TypeInfo> KnownViews { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected async Task<TResult> Navigate<T, TResult>(
            CancellationToken cancellationToken = default, params object[] args)
            where T : ViewModel<TResult>
        {
            if (!(Activator.CreateInstance(typeof(T), args) is ViewModel<TResult> vm))
                throw new ArgumentException();
            if (cancellationToken != default)
                cancellationToken.Register(() => vm.Close(default));
            var tcs = new TaskCompletionSource<TResult>();
            vm.CloseCompletionSource = tcs;
            if (!KnownViews.TryGetValue(typeof(T), out var page)) return default;
            if (!await Frame.NavigateAsync(page, vm)) return default;
            try
            {
                return await tcs.Task;
            }
            catch
            {
                return default;
            }
        }

        protected async Task Navigate<T>(params object[] args) where T : class
        {
            var vmType = typeof(T);
            var pInfo = typeof(T).GetTypeInfo();
            var uwpPage = typeof(HentaiPage).GetTypeInfo();
            if (pInfo.IsSubclassOf(typeof(ViewModel)) && KnownViews.TryGetValue(vmType, out var pageInfo))
            {
                var vm = Activator.CreateInstance(vmType, args) as ViewModel;
                await Frame.NavigateAsync(pageInfo, vm);
            }
            if (pInfo.IsAssignableFrom(uwpPage) || pInfo.IsSubclassOf(typeof(HentaiPage)))
                await Frame.NavigateAsync(vmType);
            throw new ArgumentException("Page Type must be based on HentaiPage");
        }

        protected IAsyncOperation<bool> RunOnUiThread(Action action)
        {
            return Window.Current.Dispatcher.TryRunAsync(CoreDispatcherPriority.Normal, () => action?.Invoke());
        }

        protected void Close()
        {
            if (Frame.CanGoBack)
                Frame.GoBackAsync();
            else
                Application.Current.Exit();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected internal virtual void OnDestory()
        {
        }

        protected internal virtual void OnStart()
        {
        }

        protected internal virtual void OnUnloaded()
        {
        }

        protected internal virtual void OnLoaded()
        {
        }
    }

    public abstract class ViewModel<TResult> : ViewModel
    {
        internal TaskCompletionSource<TResult> CloseCompletionSource { get; set; }

        protected internal void Close(TResult result)
        {
            CloseCompletionSource?.TrySetResult(result);
            Close();
        }

        protected internal override void OnDestory()
        {
            if (CloseCompletionSource != null && !CloseCompletionSource.Task.IsCompleted &&
                !CloseCompletionSource.Task.IsFaulted)
                CloseCompletionSource.TrySetCanceled();
            base.OnDestory();
        }
    }
}