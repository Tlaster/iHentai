using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace iHentai.Mvvm
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class PageAttribute : Attribute
    {
        public PageAttribute(Type pageType)
        {
            PageType = pageType;
        }

        public Type PageType { get; }
    }

    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected async Task<TResult> Navigate<T, TResult>(
            CancellationToken cancellationToken = default, params object[] args)
            where T : ViewModel<TResult>
        {
            var attr = typeof(T).GetTypeInfo().GetCustomAttribute<PageAttribute>();
            if (!(Activator.CreateInstance(typeof(T), args) is ViewModel<TResult> vm))
                throw new ArgumentException();
            if (cancellationToken != default)
                cancellationToken.Register(() => vm.Close(default));
            var tcs = new TaskCompletionSource<TResult>();
            vm.CloseCompletionSource = tcs;
            await NavigationService.Navigate(attr.PageType, vm);
            try
            {
                return await tcs.Task;
            }
            catch
            {
                return default;
            }
        }

        protected void Navigate<T>(params object[] args) where T : class
        {
            NavigationService.NavigateViewModel<T>(args);
        }

        protected IAsyncOperation<bool> RunOnUiThread(Action action)
        {
            return Window.Current.Dispatcher.TryRunAsync(CoreDispatcherPriority.Normal, () => action?.Invoke());
        }

        protected void Close()
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
            else
                Application.Current.Exit();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected internal virtual void Init()
        {
            
        }

        protected internal virtual void OnDestory()
        {
            
        }

        protected internal virtual void OnStart()
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