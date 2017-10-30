using Xamarin.Forms;

namespace iHentai.Core.Common
{
    public class GalleryCellSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var name = item.GetType().FullName + ".DataTemplate";
            var res = Application.Current.Resources[name] as DataTemplate;
            return res;
        }
    }
}