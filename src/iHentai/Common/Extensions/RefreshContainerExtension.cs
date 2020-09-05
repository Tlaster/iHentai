namespace iHentai.Common.Extensions
{
    internal class RefreshContainerExtension
    {
        //public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.RegisterAttached(
        //    "ItemsSource", typeof(object), typeof(RefreshContainerExtension), new PropertyMetadata(default(object)));

        //public static void SetItemsSource(DependencyObject element, object value)
        //{
        //    if (!(element is RefreshContainer refreshContainer))
        //    {
        //        return;
        //    }

        //    if (GetItemsSource(element) is IncrementalLoadingCollection<>)
        //    {
        //        refreshContainer.RefreshRequested -= RefreshContainerOnRefreshRequested;
        //    }
        //    element.SetValue(ItemsSourceProperty, value);
        //    if (value is IncrementalLoadingCollection<> source)
        //    {
        //        refreshContainer.RefreshRequested += RefreshContainerOnRefreshRequested;
        //        source.RefreshAsync();
        //    }
        //}

        //private static async void RefreshContainerOnRefreshRequested(RefreshContainer sender, RefreshRequestedEventArgs args)
        //{
        //    if (GetItemsSource(sender) is IncrementalLoadingCollection<> source)
        //    {
        //        using var def = args.GetDeferral();
        //        await source.RefreshAsync();
        //        def.Complete();
        //    }
        //}

        //public static object GetItemsSource(DependencyObject element)
        //{
        //    return (object) element.GetValue(ItemsSourceProperty);
        //}
    }
}