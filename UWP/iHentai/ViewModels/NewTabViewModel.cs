using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHentai.Activities;
using iHentai.Activities.EHentai;
using iHentai.Activities.Manhuagui;

namespace iHentai.ViewModels
{
    class NewTabViewModel : TabViewModelBase
    {
        public NewTabViewModel()
        {
            Title = "New Tab";
        }

        public Dictionary<string, Type> Services { get; } = new Dictionary<string, Type>
        {
            { "E-Hentai", typeof(GalleryActivity) },
            { "exHentai", typeof(LoginActivity) },
            { "Manhuagui", typeof(ManhuaguiUpdateActivity) }
        };
    }
}
