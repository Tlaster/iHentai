using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

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
        internal INavigation Navigation;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected async Task<TResult> Navigate<T, TResult>(
            CancellationToken cancellationToken = default, params object[] args)
            where T : ViewModel<TResult>
        {
            var attr = typeof(T).GetCustomAttribute<PageAttribute>();
            if (!(Activator.CreateInstance(typeof(T), args) is ViewModel<TResult> vm))
                throw new ArgumentException();
            var page = Activator.CreateInstance(attr.PageType);
            if (!(page is Page))
                throw new ArgumentException();
            if (cancellationToken != default)
                cancellationToken.Register(() => vm.Close(default));
            var tcs = new TaskCompletionSource<TResult>();
            vm.CloseCompletionSource = tcs;
            (page as BindableObject).BindingContext = vm;
            if (Navigation == null)
                throw new InvalidOperationException("Xamarin Forms Navigation Service not found");
            await Navigation.PushAsync(page as Page);
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
            object page;
            var pageType = typeof(T);
            var pInfo = pageType.GetTypeInfo();
            var xfPage = typeof(Page).GetTypeInfo();
            var attr = pageType.GetCustomAttribute<PageAttribute>();

            if (pInfo.IsSubclassOf(typeof(ViewModel)) && attr != null)
            {
                var vm = Activator.CreateInstance(pageType, args) as ViewModel;
                page = Activator.CreateInstance(attr.PageType);
                if (!(page is Page))
                    throw new ArgumentException();
                (page as BindableObject).BindingContext = vm;
            }
            else if (pInfo.IsAssignableFrom(xfPage) || pInfo.IsSubclassOf(typeof(Page)))
            {
                page = Activator.CreateInstance(pageType, args);
            }
            else
            {
                throw new ArgumentException("Page Type must be based on Xamarin.Forms.Page");
            }
            if (Navigation == null)
                throw new InvalidOperationException("Xamarin Forms Navigation Service not found");
            Navigation.PushAsync(page as Page);
        }

        protected internal virtual void Appearing()
        {
        }

        protected internal virtual void Disappearing()
        {
        }

        protected internal virtual void Destory()
        {
        }

        protected internal virtual void Create()
        {
        }

        protected internal virtual void Init()
        {
        }

        protected virtual void Close()
        {
            Navigation.PopAsync();
        }

        protected void RunOnUiThread(Action action)
        {
            Device.BeginInvokeOnMainThread(action);
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

        protected internal override void Destory()
        {
            if (CloseCompletionSource != null && !CloseCompletionSource.Task.IsCompleted &&
                !CloseCompletionSource.Task.IsFaulted)
                CloseCompletionSource.TrySetCanceled();
            base.Destory();
        }
    }
}