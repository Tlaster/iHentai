using System;
using Windows.UI.Xaml.Navigation;

namespace iHentai.Paging
{
    public class HentaiNavigatingCancelEventArgs
    {
        public bool Cancel { get; set; }


        public object Content { get; internal set; }


        public NavigationMode NavigationMode { get; internal set; }


        public Type SourcePageType { get; internal set; }


        public object Parameter { get; internal set; }
    }
}