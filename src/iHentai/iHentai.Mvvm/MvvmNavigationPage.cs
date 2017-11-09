using System;
using Xamarin.Forms;

namespace iHentai.Mvvm
{
    public class MvvmNavigationPage : NavigationPage
    {
        public MvvmNavigationPage(Page page) : base(page)
        {
            Pushed += OnPushed;
            Popped += OnPopped;
        }

        private void OnPushed(object sender, NavigationEventArgs e)
        {
            if (e.Page is MvvmPage page)
            {
                page.OnCreate();
            }
        }

        private void OnPopped(object sender, NavigationEventArgs e)
        {
            if (e.Page is MvvmPage page)
            {
                page.OnDestory();
            }
        }
    }
}