using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using iHentai.Basic.Helpers;
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

        public ICommand AddCommand => new RelayCommand(() =>
        {
            Source.Add(new TabViewModel(_rootType));
        });
    }

    public class TabViewModel
    {
        public string Title { get; set; } = "Title";
        public Type RootType { get; }

        public TabViewModel(Type rootType)
        {
            RootType = rootType;
        }
    }
}
