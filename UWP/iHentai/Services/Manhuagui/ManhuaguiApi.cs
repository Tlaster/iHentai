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
        public virtual string Host => "https://m.manhuagui.com";
        public async Task<List<ManhuaguiGallery>> Update(int page = 1)
        {
            return await $"{Host}/update/".SetQueryParams(new
            {
                page,
                ajax = 1,
                order = 1
            }).GetHtmlAsync<List<ManhuaguiGallery>>();
        }

        public async Task<ManhuaguiGalleryDetail> Detail(string path)
        {
            return await new Url($"{Host}{path}")
                .GetHtmlAsync<ManhuaguiGalleryDetail>();
        }
    }
}
