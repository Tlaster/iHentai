using System;
using System.Linq;
using System.Reflection;
using iHentai.Services.Core;
using iHentai.Services.Core.Common;
using iHentai.Services.EHentai.Models;

namespace iHentai.Services.EHentai
{
    public class SearchOption : SearchOptionBase
    {
        [BoolValue("f_doujinshi")]
        public bool Doujinshi { get; set; }

        [BoolValue("f_manga")]
        public bool Manga { get; set; }

        [BoolValue("f_artistcg")]
        public bool ArtistCG { get; set; }

        [BoolValue("f_gamecg")]
        public bool GameCG { get; set; }

        [BoolValue("f_western")]
        public bool Western { get; set; }

        [BoolValue("f_non-h")]
        public bool NonH { get; set; }

        [BoolValue("f_imageset")]
        public bool ImageSet { get; set; }

        [BoolValue("f_cosplay")]
        public bool Cosplay { get; set; }

        [BoolValue("f_asianporn")]
        public bool AsianPorn { get; set; }

        [BoolValue("f_misc")]
        public bool Misc { get; set; }

        [BoolValue("advsearch")]
        public bool AdvSearch { get; set; }

        [BoolValue("", Separator = "", OnValue = "f_sname=on")]
        public bool SearchName { get; set; }

        [BoolValue("", Separator = "", OnValue = "f_stags=on")]
        public bool SearchTags { get; set; }

        [BoolValue("", Separator = "", OnValue = "f_sdesc=on")]
        public bool SearchDescription { get; set; }

        [BoolValue("", Separator = "", OnValue = "f_storr=on")]
        public bool SearchTorrentFileNames { get; set; }

        [BoolValue("", Separator = "", OnValue = "f_sto=on")]
        public bool OnlyShowWithTorrents { get; set; }

        [BoolValue("", Separator = "", OnValue = "f_sdt1=on")]
        public bool SearchLowPowerTags { get; set; }

        [BoolValue("", Separator = "", OnValue = "f_sdt2=on")]
        public bool SearchDownvotedTags { get; set; }

        [BoolValue("", Separator = "", OnValue = "f_sh=on")]
        public bool ShowExpunged { get; set; }

        [BoolValue("", Separator = "", OnValue = "f_sr=on")]
        public bool EnableMinimumRating { get; set; }

        [IntValue("f_srdd")]
        public int MinimumRating { get; set; } = 2;
        
        [StringValue("f_search")]
        public override string Keyword { get; set; }

    }
}