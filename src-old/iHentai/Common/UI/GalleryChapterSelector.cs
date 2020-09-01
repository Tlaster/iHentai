using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace iHentai.Common.UI
{
    class GalleryChapterSelector : DataTemplateSelector
    {
        private static readonly Dictionary<Type, DataTemplate> _mapping = new Dictionary<Type, DataTemplate>();
        private static readonly DataTemplate _defaultTemplate;

        static GalleryChapterSelector()
        {
            _defaultTemplate = Application.Current.Resources["DefaultChapterTemplate"] as DataTemplate;
        }
        
        public static void AddMapping(DataTemplate template, Type type)
        {
            _mapping.Add(type, template);
        }
        
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item == null)
            {
                return _defaultTemplate;
            }

            if (_mapping.ContainsKey(item.GetType()))
            {
                return _mapping[item.GetType()];
            }

            return _defaultTemplate;
        }
    }
}
