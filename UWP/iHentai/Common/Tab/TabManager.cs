using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using iHentai.Activities;
using iHentai.Controls.Paging;

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

        public void Remove(int index)
        {
            TabItems.RemoveAt(index);
        }

        public void AddDefault()
        {
            TabItems.Add(new ActivityTabItem(typeof(NewTabActivity)));
        }
    }

    public interface ITabViewModel
    {
        string Title { get; }
        bool IsLoading { get; }
    }

    public interface IHistoricalTabItem
    {
        bool CanGoBack { get; }
        void GoBack();
    }

    public interface ITabItem
    {
        ITabViewModel TabViewModel { get; }
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

        public ITabViewModel TabViewModel { get; internal set; }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    internal class TabActivityContainer : ActivityContainer, IHistoricalTabItem
    {
        public static readonly DependencyProperty ActivityTabItemProperty = DependencyProperty.Register(
            nameof(ActivityTabItem), typeof(ActivityTabItem), typeof(TabActivityContainer),
            new PropertyMetadata(default(ActivityTabItem), PropertyChangedCallback));

        public TabActivityContainer()
        {
            Navigated += OnNavigated;
        }

        public ActivityTabItem ActivityTabItem
        {
            get => (ActivityTabItem) GetValue(ActivityTabItemProperty);
            set => SetValue(ActivityTabItemProperty, value);
        }

        private void OnNavigated(object sender, EventArgs e)
        {
            if (CurrentActivity is TabActivity tabActivity && ActivityTabItem != null)
            {
                ActivityTabItem.TabViewModel = tabActivity.TabViewModel;
            }
        }

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabActivityContainer view)
            {
                if (e.Property == ActivityTabItemProperty)
                {
                    view.OnTabItemChanged(e.OldValue as ActivityTabItem, e.NewValue as ActivityTabItem);
                }
            }
        }

        private void OnTabItemChanged(ActivityTabItem oldValue, ActivityTabItem newValue)
        {
            newValue?.Also(it =>
            {
                Navigate(it.TargetActivity, it.Parameter);
            });
            OnNavigated(this, EventArgs.Empty);
        }

        void IHistoricalTabItem.GoBack()
        {
            if (CanGoBack)
            {
                GoBack();
            }
        }
    }

    internal class TabActivity : Activity
    {
        public virtual ITabViewModel TabViewModel { get; }
    }
}