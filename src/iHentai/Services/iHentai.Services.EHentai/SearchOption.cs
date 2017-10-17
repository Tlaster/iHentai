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
//        public OptionModel<string, string> KeyWord { get; } = new OptionModel<string, string>("f_search", null);
//
//        public OptionModel<CategoryFlags, bool> Doujinshi { get; } = new OptionModel<CategoryFlags, bool>(CategoryFlags.Doujinshi, true);
//        public OptionModel<CategoryFlags, bool> Manga { get; } = new OptionModel<CategoryFlags, bool>(CategoryFlags.Manga, true);
//        public OptionModel<CategoryFlags, bool> ArtistCG { get; } = new OptionModel<CategoryFlags, bool>(CategoryFlags.ArtistCG, true);
//        public OptionModel<CategoryFlags, bool> GameCG { get; } = new OptionModel<CategoryFlags, bool>(CategoryFlags.GameCG, true);
//        public OptionModel<CategoryFlags, bool> Western { get; } = new OptionModel<CategoryFlags, bool>(CategoryFlags.Western, true);
//        public OptionModel<CategoryFlags, bool> NonH { get; } = new OptionModel<CategoryFlags, bool>(CategoryFlags.Nonh, true);
//        public OptionModel<CategoryFlags, bool> ImageSet { get; } = new OptionModel<CategoryFlags, bool>(CategoryFlags.ImageSet, true);
//        public OptionModel<CategoryFlags, bool> Cosplay { get; } = new OptionModel<CategoryFlags, bool>(CategoryFlags.Cosplay, true);
//        public OptionModel<CategoryFlags, bool> AsianPorn { get; } = new OptionModel<CategoryFlags, bool>(CategoryFlags.AsianPorn, true);
//        public OptionModel<CategoryFlags, bool> Misc { get; } = new OptionModel<CategoryFlags, bool>(CategoryFlags.Misc, true);
//
//        public OptionModel<string, bool> AdvSearch { get; } = new OptionModel<string, bool>("advsearch", false);
//        public OptionModel<string, bool> SearchName { get; } = new OptionModel<string, bool>("f_sname=on", true);
//        public OptionModel<string, bool> SearchTags { get; } = new OptionModel<string, bool>("f_stags=on", true);
//        public OptionModel<string, bool> SearchDescription { get; } = new OptionModel<string, bool>("f_sdesc=on", false);
//        public OptionModel<string, bool> SearchTorrentFileNames { get; } = new OptionModel<string, bool>("f_storr=on", false);
//        public OptionModel<string, bool> OnlyShowWithTorrents { get; } = new OptionModel<string, bool>("f_sto=on", false);
//        public OptionModel<string, bool> SearchLowPowerTags { get; } = new OptionModel<string, bool>("f_sdt1=on", false);
//        public OptionModel<string, bool> SearchDownvotedTags { get; } = new OptionModel<string, bool>("f_sdt2=on", false);
//        public OptionModel<string, bool> ShowExpunged { get; } = new OptionModel<string, bool>("f_sh=on", false);
//        public OptionModel<string, bool> EnableMinimumRating { get; } = new OptionModel<string, bool>("f_sr=on", false);
//
//        public OptionModel<string, int> MinimumRating { get; } = new OptionModel<string, int>("f_srdd", 2);
//
//
//        public SearchOption() : this(null)
//        {
//        }
//
//        public SearchOption(string keyword)
//        {
//            KeyWord.Value = keyword;
//        }
        
//        public override string ToString() => $"&{string.Join("&", typeof(SearchOption).GetRuntimeProperties().Where(item => item.PropertyType.GetTypeInfo().IsGenericType).Select(item => item.GetValue(this).ToString()).Where(item => item != null))}&f_apply=Apply+Filter";    
        
        [Query("f_search")]
        public string Keyword { get; set; }

        public string ToQueryString()
        {
            throw new NotImplementedException();
//            $"{string.Join("&", typeof(SearchOption).GetRuntimeProperties().Where(item => Attribute.IsDefined(item, typeof(QueryAttribute))).Select(item => item.GetValue(this).ToString()))}";
        }
    }
}