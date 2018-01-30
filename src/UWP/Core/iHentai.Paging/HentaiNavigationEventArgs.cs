using System;
using Windows.UI.Xaml.Navigation;

namespace iHentai.Paging
{
    public class HentaiNavigationEventArgs
    {
        public object Content { get; internal set; }


        public object Parameter { get; internal set; }


        public Type SourcePageType { get; internal set; }


        public NavigationMode NavigationMode { get; internal set; }


        public object[] Parameters => Parameter as object[];


        public T GetParameter<T>(int index)
        {
            return (T) Parameters[index];   
        }


        public T GetParameter<T>()
        {
            return (T) Parameter;
        }
    }
}