using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using PropertyChanged;

namespace iHentai.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class RootViewModel
    {
        private readonly Type _rootType;

        public RootViewModel(Type rootType)
        {
            _rootType = rootType;
            Source.Add(new TabViewModel(rootType));
        }

        public ObservableCollection<TabViewModel> Source { get; } = new ObservableCollection<TabViewModel>();

        public void AddTab()
        {
            Source.Add(new TabViewModel(_rootType));
        }

        public void TabClosed()
        {
            if (!Source.Any()) Application.Current.Exit();
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class TabViewModel
    {
        public TabViewModel(Type rootType)
        {
            RootType = rootType;
        }

        public Type RootType { get; }
    }
}