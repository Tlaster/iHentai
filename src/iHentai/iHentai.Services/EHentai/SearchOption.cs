using iHentai.Services.Core;
using iHentai.Services.Core.Common.Attributes;

namespace iHentai.Services.EHentai
{
    public class SearchOption : SearchOptionBase
    {
        [BoolValue("f_doujinshi", OnValue = "1", OffValue = "0")]
        public bool Doujinshi { get; set; }

        [BoolValue("f_manga", OnValue = "1", OffValue = "0")]
        public bool Manga { get; set; }

        [BoolValue("f_artistcg", OnValue = "1", OffValue = "0")]
        public bool ArtistCG { get; set; }

        [BoolValue("f_gamecg", OnValue = "1", OffValue = "0")]
        public bool GameCG { get; set; }

        [BoolValue("f_western", OnValue = "1", OffValue = "0")]
        public bool Western { get; set; }

        [BoolValue("f_non-h", OnValue = "1", OffValue = "0")]
        public bool NonH { get; set; }

        [BoolValue("f_imageset", OnValue = "1", OffValue = "0")]
        public bool ImageSet { get; set; }

        [BoolValue("f_cosplay", OnValue = "1", OffValue = "0")]
        public bool Cosplay { get; set; }

        [BoolValue("f_asianporn", OnValue = "1", OffValue = "0")]
        public bool AsianPorn { get; set; }

        [BoolValue("f_misc", OnValue = "1", OffValue = "0")]
        public bool Misc { get; set; }

        [BoolValue("advsearch", OnValue = "1", OffValue = "0")]
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