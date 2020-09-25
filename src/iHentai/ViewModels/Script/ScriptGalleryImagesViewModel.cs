using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHentai.Services.Models.Script;

namespace iHentai.ViewModels.Script
{
    class ScriptGalleryImagesViewModel
    {
        public ScriptGalleryImagesViewModel(IEnumerable<ScriptGalleryThumb> images)
        {
            Images = images;
        }

        public IEnumerable<ScriptGalleryThumb> Images { get; }
    }
}
