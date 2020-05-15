using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using PropertyChanged;

namespace iHentai.Common.Helpers
{
    internal class ThemeManager : INotifyPropertyChanged
    {
        public ThemeManager(FrameworkElement rootView)
        {
            RootView = rootView;
        }

        public ElementTheme Theme
        {
            get => RootView.RequestedTheme;
            set => RootView.RequestedTheme = value;
        }

        public FrameworkElement RootView { get; }

        [DependsOn(nameof(Theme))]
        public bool IsDarkTheme
        {
            get => Theme == ElementTheme.Dark;
            set => Theme = ElementTheme.Dark;
        }

        [DependsOn(nameof(Theme))]
        public bool IsLightTheme
        {
            get => Theme == ElementTheme.Light;
            set => Theme = ElementTheme.Light;
        }

        [DependsOn(nameof(Theme))]
        public bool IsDefaultTheme
        {
            get => Theme == ElementTheme.Default;
            set => Theme = ElementTheme.Default;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}