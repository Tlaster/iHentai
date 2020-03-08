using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Bug10.Paging;
using iHentai.Activities;

namespace iHentai.Common.Tab
{
    internal class TabManager
    {
        public TabManager()
        {
            AddDefault();
        }

        public ObservableCollection<ITabItem> TabItems { get; } = new ObservableCollection<ITabItem>();

        public int Count => TabItems.Count;

        public void Add(ITabItem item)
        {
            TabItems.Add(item);
        }

        public void Remove(ITabItem item)
        {
            TabItems.Remove(item);
        }

        public void AddDefault()
        {
            TabItems.Add(new ActivityTabItem(typeof(NewTabActivity)));
        }
    }

    internal interface ITabItem
    {
        string Icon { get; }
        string Title { get; }
        bool IsLoading { get; }
    }

    internal class TabItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ActivityTabItemTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is ActivityTabItem)
            {
                return ActivityTabItemTemplate;
            }
            return base.SelectTemplateCore(item, container);
        }
    }

    public class ActivityTabItem : ITabItem, INotifyPropertyChanged
    {
        public ActivityTabItem(Type targetActivity, object parameter = null)
        {
            TargetActivity = targetActivity;
            Parameter = parameter;
        }

        public Type TargetActivity { get; }
        public object Parameter { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Icon { get; internal set; }
        public string Title { get; internal set; }
        public bool IsLoading { get; internal set; }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    internal class TabActivityContainer : ActivityContainer
    {
        public TabActivityContainer()
        {
            Navigated += OnNavigated;
        }

        private void OnNavigated(object sender, EventArgs e)
        {
            if (CurrentActivity is TabActivity tabActivity)
            {
                ActivityTabItem.Title = tabActivity.Title;
            }
            else
            {
                ActivityTabItem.Title = string.Empty;
            }
        }

        public static readonly DependencyProperty ActivityTabItemProperty = DependencyProperty.Register(
            nameof(ActivityTabItem), typeof(ActivityTabItem), typeof(TabActivityContainer),
            new PropertyMetadata(default(ActivityTabItem), PropertyChangedCallback));

        public ActivityTabItem ActivityTabItem
        {
            get => (ActivityTabItem) GetValue(ActivityTabItemProperty);
            set => SetValue(ActivityTabItemProperty, value);
        }

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabActivityContainer view)
            {
                if (e.Property == ActivityTabItemProperty)
                {
                    view.OnTabItemChanged(e.NewValue as ActivityTabItem);
                }
            }
        }

        private void OnTabItemChanged(ActivityTabItem newValue)
        {
            Navigate(newValue.TargetActivity, newValue.Parameter);
            OnNavigated(this, EventArgs.Empty);
        }

    }

    internal class TabActivity : Activity, ITabItem
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title), typeof(string), typeof(TabActivity),
            new PropertyMetadata(default(string), PropertyChangedCallback));

        public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register(
            nameof(IsLoading), typeof(bool), typeof(TabActivity),
            new PropertyMetadata(default(bool), PropertyChangedCallback));

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            nameof(Icon), typeof(string), typeof(TabActivity),
            new PropertyMetadata(default(string), PropertyChangedCallback));

        public string Title
        {
            get => (string) GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public bool IsLoading
        {
            get => (bool) GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, value);
        }

        public string Icon
        {
            get => (string) GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabActivity view)
            {
                if (view.Container is TabActivityContainer tabActivityContainer)
                {
                    if (e.Property == TitleProperty)
                    {
                        tabActivityContainer.ActivityTabItem.Title = e.NewValue as string;
                    }

                    if (e.Property == IsLoadingProperty)
                    {
                        tabActivityContainer.ActivityTabItem.IsLoading = e.NewValue is bool value && value;
                    }

                    if (e.Property == IconProperty)
                    {
                        tabActivityContainer.ActivityTabItem.Icon = e.NewValue as string;
                    }
                }
            }
        }
    }
}