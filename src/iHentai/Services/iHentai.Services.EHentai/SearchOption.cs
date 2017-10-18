using System;
using System.Linq;
using System.Reflection;
using iHentai.Services.Core;
using iHentai.Services.Core.Common;
using iHentai.Services.EHentai.Models;

namespace iHentai.Services.EHentai
{
    public class SearchOption : ISearchOption
    {
        [Query("f_doujinshi")]
        public bool Doujinshi { get; set; }

        [Query("f_manga")]
        public bool Manga { get; set; }

        [Query("f_artistcg")]
        public bool ArtistCG { get; set; }

        [Query("f_gamecg")]
        public bool GameCG { get; set; }

        [Query("f_western")]
        public bool Western { get; set; }

        [Query("f_non-h")]
        public bool NonH { get; set; }

        [Query("f_imageset")]
        public bool ImageSet { get; set; }

        [Query("f_cosplay")]
        public bool Cosplay { get; set; }

        [Query("f_asianporn")]
        public bool AsianPorn { get; set; }

        [Query("f_misc")]
        public bool Misc { get; set; }

        [Query("advsearch")]
        public bool AdvSearch { get; set; }

        [Query("f_sname=on", HasValue = false)]
        public bool SearchName { get; set; }

        [Query("f_stags=on", HasValue = false)]
        public bool SearchTags { get; set; }

        [Query("f_sdesc=on", HasValue = false)]
        public bool SearchDescription { get; set; }

        [Query("f_storr=on", HasValue = false)]
        public bool SearchTorrentFileNames { get; set; }

        [Query("f_sto=on", HasValue = false)]
        public bool OnlyShowWithTorrents { get; set; }

        [Query("f_sdt1=on", HasValue = false)]
        public bool SearchLowPowerTags { get; set; }

        [Query("f_sdt2=on", HasValue = false)]
        public bool SearchDownvotedTags { get; set; }

        [Query("f_sh=on", HasValue = false)]
        public bool ShowExpunged { get; set; }

        [Query("f_sr=on", HasValue = false)]
        public bool EnableMinimumRating { get; set; }

        [Query("f_srdd")]
        public int MinimumRating { get; set; } = 2;
        
        [Query("f_search")]
        public string Keyword { get; set; }

        public string ToQueryString()
        {
            return $"{string.Join("&", typeof(SearchOption).GetRuntimeProperties().Where(item => Attribute.IsDefined(item, typeof(QueryAttribute))).Select(GetValueFromPropertyInfo))}";
        }

        private string GetValueFromPropertyInfo(PropertyInfo info)
        {
            var item = info.GetValue(this);
            var attribute = info.GetCustomAttribute<QueryAttribute>();
            switch (item)
            {
                case bool value:
                    return attribute.HasValue ? $"{attribute.Key}={Convert.ToInt32(value)}" : $"{(value ? attribute.Key : "")}";
                default:
                    return $"{attribute.Key}={item}";
            }
        }
    }
}