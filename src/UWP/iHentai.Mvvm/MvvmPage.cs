using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace iHentai.Mvvm
{
    public class MvvmPage : Page, IDisposable
    {
        private MvvmBundle _bundle;

        public MvvmPage()
        {
            Loading += OnLoading;
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        public ViewModel ViewModel
        {
            get => _bundle?.ViewModel;
            set => _bundle.ViewModel = value;
        }

        public void Dispose()
        {
            Debug.WriteLine($"{GetType().Name}: Dispose");
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            OnStop();
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            OnResume();
        }

        private void OnLoading(FrameworkElement sender, object args)
        {
            OnStart();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var isNewIntent = _bundle != null && e.NavigationMode == NavigationMode.New;
            var oldIntent = _bundle;
            _bundle = e.Parameter as MvvmBundle;
            var newIntent = _bundle;
            switch (e.NavigationMode)
            {
                case NavigationMode.New:
                    OnCreate();
                    break;
                case NavigationMode.Back:
                    RestoreState(_bundle?.PageState);
                    _bundle?.PageState.Clear();
                    OnRestart();
                    break;
                case NavigationMode.Forward:
                    break;
                case NavigationMode.Refresh:
                    break;
                default:
                    break;
            }
            if (isNewIntent)
                OnNewIntent(oldIntent?.ViewModel, newIntent?.ViewModel);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            switch (e.NavigationMode)
            {
                case NavigationMode.New:
                    break;
                case NavigationMode.Back:
                    OnClose();
                    break;
                case NavigationMode.Forward:
                    break;
                case NavigationMode.Refresh:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            switch (e.NavigationMode)
            {
                case NavigationMode.New:
                    OnPause();
                    SaveState(_bundle?.PageState);
                    break;
                case NavigationMode.Back:
                    OnDestory();
                    break;
                case NavigationMode.Forward:
                    break;
                case NavigationMode.Refresh:
                    break;
                default:
                    break;
            }
        }

        protected virtual void OnCreate()
        {
            Debug.WriteLine($"{GetType().Name}: OnCreate");
            ViewModel?.OnCreate();
        }

        protected virtual void OnNewIntent(ViewModel oldViewModel, ViewModel newViewModel)
        {
            Debug.WriteLine($"{GetType().Name}: OnNewIntent");
        }

        protected virtual void OnRestart()
        {
            Debug.WriteLine($"{GetType().Name}: OnRestart");
        }

        protected virtual void OnStart()
        {
            Debug.WriteLine($"{GetType().Name}: OnStart");
        }

        protected virtual void OnResume()
        {
            Debug.WriteLine($"{GetType().Name}: OnResume");
        }

        protected virtual void OnStop()
        {
            Debug.WriteLine($"{GetType().Name}: OnStop");
        }

        protected virtual void OnPause()
        {
            Debug.WriteLine($"{GetType().Name}: OnPause");
        }

        protected virtual void OnClose()
        {
            Debug.WriteLine($"{GetType().Name}: OnClose");
        }

        protected virtual void OnDestory()
        {
            Debug.WriteLine($"{GetType().Name}: OnDestory");
            ViewModel?.OnDestory();
        }

        protected virtual void RestoreState(Dictionary<string, object> bundleState)
        {
            Debug.WriteLine($"{GetType().Name}: RestoreState");
        }

        protected virtual void SaveState(Dictionary<string, object> bundleState)
        {
            Debug.WriteLine($"{GetType().Name}: SaveState");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Debug.WriteLine($"{GetType().Name}: Disposing");
                Loading -= OnLoading;
                Loaded -= OnLoaded;
                Unloaded -= OnUnloaded;
            }
        }

        ~MvvmPage()
        {
            Debug.WriteLine($"{GetType().Name}: dctor");
            Dispose(false);
        }
    }

    public class MvvmPage<T> : MvvmPage where T : ViewModel
    {
        public new T ViewModel
        {
            get => base.ViewModel as T;
            set => base.ViewModel = value;
        }
    }
}