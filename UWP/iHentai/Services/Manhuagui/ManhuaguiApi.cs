using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using iHentai.Services.EHentai;
using iHentai.Services.Manhuagui.Model;

namespace iHentai.Services.Manhuagui
{
    class ManhuaguiApi
    {
        public virtual string Host => "https://www.manhuagui.com/";
        public async Task<List<ManhuaguiGallery>> Update()
        {
            var result = await new Url($"{Host}update/").GetHtmlAsync<ManhuaguiGalleryUpdate>();
            return result.Items;
        }
    }
}
