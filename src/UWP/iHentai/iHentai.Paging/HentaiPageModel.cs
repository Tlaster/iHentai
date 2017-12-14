using System;

namespace iHentai.Paging
{
    internal class HentaiPageModel
    {
        public HentaiPageModel(Type pageType, object parameter)
        {
            Type = pageType;
            Parameter = parameter;
            PageKey = Guid.NewGuid().ToString();
        }

        public object Parameter { get; internal set; }

        public HentaiPage Page { get; private set; }

        public Type Type { get; }

        private string PageKey { get; }

        internal HentaiPage GetPage(HentaiFrame frame)
        {
            if (Page != null) return Page;
            if (!(Activator.CreateInstance(Type) is HentaiPage page))
                throw new InvalidOperationException(
                    $"The base type is not an {nameof(HentaiPage)}. Change the base type from Page to {nameof(HentaiPage)}. ");

            page.SetFrame(frame, PageKey);
            Page = page;

            return Page;
        }


        internal void ReleasePage()
        {
            //Page.OnDestory();
            Page = null;
        }
    }
}