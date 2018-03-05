using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using iHentai.Basic.Extensions;
using iHentai.Basic.Helpers;
using iHentai.Paging;

namespace iHentai.Mvvm
{
    public class MvvmFrame : HentaiFrame, INotifyPropertyChanged
    {
        public static readonly DependencyProperty TargetSourcePageProperty = DependencyProperty.Register(
            nameof(TargetSourcePage), typeof(Type), typeof(MvvmFrame),
            new PropertyMetadata(default(Type), OnTargetSourcePageChanged));

        private long _token;

        public Type TargetSourcePage
        {
            get => (Type) GetValue(TargetSourcePageProperty);
            set => SetValue(TargetSourcePageProperty, value);
        }

        public MvvmPage CurrentMvvmPage => CurrentPage as MvvmPage;

        public string CurrentPageTitle => (CurrentPage as MvvmPage)?.Title ?? Package.Current.DisplayName;

        public ICommand GoBackCommand => new RelayCommand(() => GoBackAsync());

        public event PropertyChangedEventHandler PropertyChanged;

        private static void OnTargetSourcePageChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is Type type) (dependencyObject as MvvmFrame)?.OnTargetSourcePageChanged(type);
        }

        private void OnTargetSourcePageChanged(Type newValue)
        {
            var info = newValue.GetTypeInfo();
            if (ReflectionHelper.ImplementsGenericDefinition(newValue, typeof(IMvvmView<>), out var vmType))
                NavigateAsync(newValue, Activator.CreateInstance(vmType.GetGenericArguments().FirstOrDefault()))
                    .FireAndForget();
            else if (info.IsSubclassOf(typeof(ViewModel)) && ViewModel.KnownViews.TryGetValue(vmType, out var pageInfo))
                NavigateAsync(pageInfo, Activator.CreateInstance(vmType)).FireAndForget();
            else
                NavigateAsync(newValue).FireAndForget();
        }

        protected override void OnCurrentPageChanged(HentaiPage currentPage, HentaiPage newPage)
        {
            base.OnCurrentPageChanged(currentPage, newPage);
            currentPage?.UnregisterPropertyChangedCallback(MvvmPage.TitleProperty, _token);

            if (newPage != null)
                _token = newPage.RegisterPropertyChangedCallback(MvvmPage.TitleProperty, OnPageTitleChanged);
        }

        private void OnPageTitleChanged(DependencyObject sender, DependencyProperty dp)
        {
            OnPropertyChanged(nameof(CurrentPageTitle));
        }

        protected override void OnNavigated(object sender, HentaiNavigationEventArgs args)
        {
            base.OnNavigated(sender, args);
            OnPropertyChanged(nameof(CanGoBack));
            OnPropertyChanged(nameof(CurrentPageTitle));
            OnPropertyChanged(nameof(CurrentMvvmPage));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}