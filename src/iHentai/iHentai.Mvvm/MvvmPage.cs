using Xamarin.Forms;

namespace iHentai.Mvvm
{
    public abstract class MvvmPage : ContentPage
    {
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            ViewModel.Navigation = Navigation;
            ViewModel.Init();
        }


        public ViewModel ViewModel
        {
            get => (ViewModel) BindingContext;
            set => BindingContext = value;
        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel?.Appearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewModel?.Disappearing();
        }
        
    }
}