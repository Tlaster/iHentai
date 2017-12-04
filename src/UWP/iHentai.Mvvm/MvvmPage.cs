using System.Diagnostics;
using Windows.UI.Xaml.Navigation;
using iHentai.Paging;

namespace iHentai.Mvvm
{
    public abstract class MvvmPage : HentaiPage
    {
        public ViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(HentaiNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel = e.Parameter as ViewModel;
            DataContext = ViewModel;
            switch (e.NavigationMode)
            {
                case NavigationMode.New:
                    OnStart();
                    break;
                case NavigationMode.Back:
                    OnRestart();
                    break;
                case NavigationMode.Forward:
                    break;
                case NavigationMode.Refresh:
                    break;
                default:
                    break;
            }
        }

        protected override void OnNavigatingFrom(HentaiNavigatingCancelEventArgs e)
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
                    break;
            }
        }

        protected override void OnNavigatedFrom(HentaiNavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            switch (e.NavigationMode)
            {
                case NavigationMode.New:
                    OnPause();
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

        protected virtual void OnStart()
        {
            Debug.WriteLine($"{GetType().Name}: ${nameof(OnStart)}");
            ViewModel?.OnStart();
        }
        
        protected virtual void OnRestart()
        {
            Debug.WriteLine($"{GetType().Name}: ${nameof(OnRestart)}");
        }
        
        protected virtual void OnPause()
        {
            Debug.WriteLine($"{GetType().Name}: ${nameof(OnPause)}");
        }

        protected virtual void OnClose()
        {
            Debug.WriteLine($"{GetType().Name}: ${nameof(OnClose)}");
        }

        protected virtual void OnDestory()
        {
            Debug.WriteLine($"{GetType().Name}: ${nameof(OnDestory)}");
            ViewModel?.OnDestory();
        }
    }
}