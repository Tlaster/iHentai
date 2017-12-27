using System;
using System.Linq;
using Windows.UI.Xaml;
using iHentai.Basic.Extensions;
using iHentai.Basic.Helpers;
using iHentai.Paging;

namespace iHentai.Mvvm
{
    public class MvvmFrame : HentaiFrame
    {
        public static readonly DependencyProperty TargetSourcePageProperty = DependencyProperty.Register(
            nameof(TargetSourcePage), typeof(Type), typeof(MvvmFrame),
            new PropertyMetadata(default(Type), OnTargetSourcePageChanged));

        public Type TargetSourcePage
        {
            get => (Type) GetValue(TargetSourcePageProperty);
            set => SetValue(TargetSourcePageProperty, value);
        }

        private static void OnTargetSourcePageChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is Type type)
            {
                (dependencyObject as MvvmFrame)?.OnTargetSourcePageChanged(type);
            }
        }

        private void OnTargetSourcePageChanged(Type newValue)
        {
            if (ReflectionHelper.ImplementsGenericDefinition(newValue, typeof(IMvvmView<>), out var vmType))
                NavigateAsync(newValue, Activator.CreateInstance(vmType.GetGenericArguments().FirstOrDefault()))
                    .FireAndForget();
            else
                NavigateAsync(newValue).FireAndForget();
        }
    }
}