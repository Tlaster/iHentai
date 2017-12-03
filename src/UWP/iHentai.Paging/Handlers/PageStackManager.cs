using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace iHentai.Paging.Handlers
{
    internal class PageStackManager
    {
        private readonly List<HentaiPageModel> _pages = new List<HentaiPageModel>();
        private int _currentIndex = -1;

        public int CurrentIndex
        {
            get { return _currentIndex; }
            private set
            {
                if (_currentIndex != value)
                {
                    _currentIndex = value;
                    
                    if (AutomaticBackButtonHandling)
                        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                            CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
                }
            }
        }

        public bool AutomaticBackButtonHandling { get; set; }

        public bool IsFirstPage => CurrentIndex == 0;

        public HentaiPageModel PreviousPage => CurrentIndex > 0 ? _pages[CurrentIndex - 1] : null;

        public HentaiPageModel CurrentPage => _pages.Count > 0 ? _pages[CurrentIndex] : null;

        public HentaiPageModel NextPage => CurrentIndex < _pages.Count - 1 ? _pages[CurrentIndex + 1] : null;

        public bool CanGoForward => CurrentIndex < _pages.Count - 1;

        public bool CanGoBack => CurrentIndex > 0;

        public IReadOnlyList<HentaiPageModel> Pages => _pages;

        public int BackStackDepth => CurrentIndex + 1;

        public HentaiPageModel GetNearestPageOfTypeInBackStack(Type pageType)
        {
            var index = CurrentIndex;
            while (index >= 0)
            {
                if (_pages[index].Type == pageType)
                    return _pages[index];
                index--;
            }
            return null;
        }

        public HentaiPageModel GetPageAt(int index)
        {
            return _pages[index];
        }

        public int GetPageIndex(HentaiPageModel pageDescription)
        {
            return _pages.IndexOf(pageDescription);
        }


        public bool RemovePageFromStack(HentaiPageModel pageDescription)
        {
            var index = GetPageIndex(pageDescription);
            if (index >= 0)
            {
                RemovePageFromStackAt(index);
                return true;
            }
            return false;
        }


        public bool RemovePageFromStackAt(int pageIndex)
        {
            if (pageIndex == CurrentIndex)
                throw new ArgumentException("The current page cannot be removed from the stack. ");

            _pages.RemoveAt(pageIndex);
            if (pageIndex < CurrentIndex)
                CurrentIndex--;

            return true;
        }

        public async Task<bool> MoveToTop(HentaiPageModel page, Func<HentaiPageModel, Task<bool>> action)
        {
            if (CurrentPage == page)
                return true;

            var index = _pages.IndexOf(page);
            if (index != -1)
            {
                _pages.RemoveAt(index);
                _currentIndex--;

                if (await action(page))
                    return true;

                _pages.Insert(index, page);
            }
            return false;
        }
        
        public void ClearBackStack()
        {
            for (var i = _currentIndex - 1; i >= 0; i--)
                RemovePageFromStackAt(i);
        }

        public void ClearForwardStack()
        {
            for (var i = _pages.Count - 1; i > CurrentIndex; i--)
                RemovePageFromStackAt(i);
        }

        public void ChangeCurrentPage(HentaiPageModel newPage, int nextPageIndex)
        {
            if (_pages.Count <= nextPageIndex)
                _pages.Add(newPage);

            CurrentIndex = nextPageIndex;
        }

        public bool CanGoBackTo(int newPageIndex)
        {
            if (newPageIndex == CurrentIndex)
                return false;

            if (newPageIndex < 0 || newPageIndex > CurrentIndex)
                return false;

            return true;
        }
    }
}