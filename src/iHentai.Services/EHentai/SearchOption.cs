using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using iHentai.Services.Core;
using iHentai.Services.Core.Attributes;

namespace iHentai.Services.EHentai
{
    public class SearchOption : ISearchOption, INotifyPropertyChanged
    {
        public bool Doujinshi { get; set; } = true;
        public bool Manga { get; set; } = true;
        public bool ArtistCG { get; set; } = true;
        public bool GameCG { get; set; } = true;
        public bool Western { get; set; } = true;
        public bool NonH { get; set; } = true;
        public bool ImageSet { get; set; } = true;
        public bool Cosplay { get; set; } = true;
        public bool AsianPorn { get; set; } = true;
        public bool Misc { get; set; } = true;

        [IntValue("f_cats")]
        public int Cats
        {
            get
            {
                //https://ehwiki.org/wiki/Gallery_Search_Engine
                var result = 1023;
                if (Doujinshi)
                {
                    result -= 2;
                }

                if (Manga)
                {
                    result -= 4;
                }

                if (ArtistCG)
                {
                    result -= 8;
                }

                if (GameCG)
                {
                    result -= 16;
                }

                if (Western)
                {
                    result -= 512;
                }

                if (NonH)
                {
                    result -= 256;
                }

                if (ImageSet)
                {
                    result -= 32;
                }

                if (Cosplay)
                {
                    result -= 64;
                }

                if (AsianPorn)
                {
                    result -= 128;
                }

                if (Misc)
                {
                    result -= 1;
                }

                return result;
            }
        }

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

        public virtual string ToSearchParameter()
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

    public class FavSearchOption : SearchOption
    {
        public override string ToSearchParameter()
        {
            return $"favcat=all&f_search={Keyword}&sn=on&st=on&sf=on";
        }
    }
}