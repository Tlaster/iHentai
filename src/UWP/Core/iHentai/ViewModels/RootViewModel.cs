using System;
using Windows.UI.Xaml;
using PropertyChanged;
using Tab;

namespace iHentai.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class RootViewModel
    {
        public RootViewModel(Type rootType)
        {
            RootType = rootType;
        }

        public Type RootType { get; }

        public void TabClosed(object sender, TabCloseEventArgs e)
        {
            if (e.TabCount == 0) Application.Current.Exit();
        }
    }
}