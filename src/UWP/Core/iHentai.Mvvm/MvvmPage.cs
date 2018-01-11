using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using iHentai.Paging;

namespace iHentai.Mvvm
{
    public abstract class MvvmPage : HentaiPage, INotifyPropertyChanged
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title), typeof(string), typeof(MvvmPage), new PropertyMetadata(default(string)));

        public MvvmPage()
        {
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        public string Title
        {
            get => (string) GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public ViewModel ViewModel { get; set; }

        protected override void SetFrame(HentaiFrame frame, string pageKey)
        {
            base.SetFrame(frame, pageKey);
            if (ViewModel != null && ViewModel.Frame == null)
                ViewModel.Frame = frame;
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ViewModel?.OnUnloaded();
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ViewModel?.OnLoaded();
        }

        protected override void OnNavigatedTo(HentaiNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel = e.Parameter as ViewModel;
            if (ViewModel != null && ViewModel.Frame == null)
                ViewModel.Frame = Frame;
            DataContext = ViewModel;
            OnPropertyChanged(nameof(ViewModel));
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
            ViewModel?.OnStart();
        }

        protected virtual void OnRestart()
        {
        }

        protected virtual void OnPause()
        {
        }

        protected virtual void OnClose()
        {
        }

        protected virtual void OnDestory()
        {
            ViewModel?.OnDestory();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}