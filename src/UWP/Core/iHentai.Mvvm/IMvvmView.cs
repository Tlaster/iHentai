namespace iHentai.Mvvm
{
    public interface IMvvmView<T> where T : ViewModel
    {
        T ViewModel { get; set; }
    }
}