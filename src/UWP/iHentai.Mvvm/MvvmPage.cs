using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace iHentai.Mvvm
{
    public class MvvmPage : Page
    {
        private MvvmBundle _bundle;

        public ViewModel ViewModel
        {
            get => _bundle?.ViewModel;
            set => _bundle.ViewModel = value;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _bundle = e.Parameter as MvvmBundle;
            switch (e.NavigationMode)
            {
                case NavigationMode.New:
                    ViewModel?.OnCreate();
                    break;
                case NavigationMode.Back:
                    RestoreState(_bundle?.PageState);
                    break;
                case NavigationMode.Forward:
                    break;
                case NavigationMode.Refresh:
                    break;
                default:
                    break;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            switch (e.NavigationMode)
            {
                case NavigationMode.New:
                    SaveState(_bundle?.PageState);
                    break;
                case NavigationMode.Back:
                    ViewModel?.OnDestory();
                    break;
                case NavigationMode.Forward:
                    break;
                case NavigationMode.Refresh:
                    break;
                default:
                    break;
            }
        }

        protected virtual void RestoreState(Dictionary<string, object> bundleState)
        {
        }


        protected virtual void SaveState(Dictionary<string, object> bundleState)
        {
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