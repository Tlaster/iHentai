using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using iHentai.Services.Core;
using iHentai.Services.Core.Attributes;

namespace iHentai.Services.EHentai
{
    public class SearchOption : ISearchOption, INotifyPropertyChanged
    {
        [BoolValue("f_doujinshi", OnValue = "1", OffValue = "0")]
        public bool Doujinshi { get; set; } = true;

        [BoolValue("f_manga", OnValue = "1", OffValue = "0")]
        public bool Manga { get; set; } = true;

        [BoolValue("f_artistcg", OnValue = "1", OffValue = "0")]
        public bool ArtistCG { get; set; } = true;

        [BoolValue("f_gamecg", OnValue = "1", OffValue = "0")]
        public bool GameCG { get; set; } = true;

        [BoolValue("f_western", OnValue = "1", OffValue = "0")]
        public bool Western { get; set; } = true;

        [BoolValue("f_non-h", OnValue = "1", OffValue = "0")]
        public bool NonH { get; set; } = true;

        [BoolValue("f_imageset", OnValue = "1", OffValue = "0")]
        public bool ImageSet { get; set; } = true;

        [BoolValue("f_cosplay", OnValue = "1", OffValue = "0")]
        public bool Cosplay { get; set; } = true;

        [BoolValue("f_asianporn", OnValue = "1", OffValue = "0")]
        public bool AsianPorn { get; set; } = true;

        [BoolValue("f_misc", OnValue = "1", OffValue = "0")]
        public bool Misc { get; set; } = true;

        [BoolValue("advsearch", OnValue = "1", OffValue = "0")]
        public bool AdvSearch { get; set; }

        [BoolValue("", Separator = "", OnValue = "f_sname=on")]
        public bool SearchName { get; set; } = true;

        [BoolValue("", Separator = "", OnValue = "f_stags=on")]
        public bool SearchTags { get; set; } = true;

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
        
        [BoolValue("", Separator = "", OnValue = "f_sfl=on")]
        public bool DisableDefaultFiltersForLanguage { get; set; }

        [BoolValue("", Separator = "", OnValue = "f_sfu=on")]
        public bool DisableDefaultFiltersForUploader { get; set; }

        [BoolValue("", Separator = "", OnValue = "f_sft=on")]
        public bool DisableDefaultFiltersForTags { get; set; }

        [BoolValue("", Separator = "", OnValue = "f_sr=on")]
        public bool EnableMinimumRating { get; set; }

        [IntValue("f_srdd")]
        public int MinimumRating { get; set; } = 2;

        [StringValue("f_search")]
        public string Keyword { get; set; }

        public string ToSearchParameter()
        {
            var values = GetType()
                .GetProperties()
                .Select(item => item.GetValueAttribute(item.GetValue(this)))
                .Where(item => !string.IsNullOrEmpty(item))
                .ToList();
            return string.Join("&", values);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}