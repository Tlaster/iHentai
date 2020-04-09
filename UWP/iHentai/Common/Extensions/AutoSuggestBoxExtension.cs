using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using iHentai.Common.Helpers;
using Microsoft.Toolkit.Helpers;

namespace iHentai.Common.Extensions
{
    class AutoSuggestBoxExtension
    {
        public static readonly DependencyProperty SuggestionNameProperty = DependencyProperty.RegisterAttached(
            "SuggestionName", typeof(string), typeof(AutoSuggestBox), new PropertyMetadata(default(string)));

        public static void SetSuggestionName(DependencyObject element, string value)
        {
            element.SetValue(SuggestionNameProperty, value);
            if (element is AutoSuggestBox view)
            {
                view.TextChanged += ViewOnTextChanged;
                view.QuerySubmitted += ViewOnQuerySubmitted;
                view.SuggestionChosen += ViewOnSuggestionChosen;
            }
        }

        private static void ViewOnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            sender.Text = args.SelectedItem?.ToString() ?? string.Empty;
        }

        private static void ViewOnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var queryText = args.ChosenSuggestion?.ToString() ?? args.QueryText;
            var name = GetSuggestionName(sender);
            if (string.IsNullOrEmpty(queryText) || string.IsNullOrEmpty(name))
            {
                return;
            }
            var current = Singleton<Settings>.Instance.Get(name + "_search_list", new List<string>());
            current.Remove(queryText);
            current.Insert(0, queryText);
            Singleton<Settings>.Instance.Set(name + "_search_list", current);
        }

        private static void ViewOnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            var name = GetSuggestionName(sender);
            if (!string.IsNullOrEmpty(name) && args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                sender.ItemsSource = Singleton<Settings>.Instance.Get(name + "_search_list", new List<string>()).Where(it => it.Contains(sender.Text)).ToList();
            }
        }

        public static string GetSuggestionName(DependencyObject element)
        {
            return (string) element.GetValue(SuggestionNameProperty);
        }
    }
}
