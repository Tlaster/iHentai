using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using iHentai.Services.Core;
using iHentai.Services.Core.Common;
using iHentai.Services.Core.Common.Attributes;
using iHentai.Services.EHentai.Models;

namespace iHentai.Services.EHentai
{
    public class UConfig : AutoString, IApiConfig
    {
        private const string UConfigItemSeparator = "_";
        
        public Dictionary<LanguageFlags, LanguageModel> Language { get; } = new Dictionary<LanguageFlags, LanguageModel>
        {
            {LanguageFlags.Japanese, new LanguageModel(0)},
            {LanguageFlags.English, new LanguageModel(1)},
            {LanguageFlags.Chinese, new LanguageModel(10)},
            {LanguageFlags.Dutch, new LanguageModel(20)},
            {LanguageFlags.French, new LanguageModel(30)},
            {LanguageFlags.German, new LanguageModel(40)},
            {LanguageFlags.Hungarian, new LanguageModel(50)},
            {LanguageFlags.Italian, new LanguageModel(60)},
            {LanguageFlags.Korean, new LanguageModel(70)},
            {LanguageFlags.Polish, new LanguageModel(80)},
            {LanguageFlags.Portuguese, new LanguageModel(90)},
            {LanguageFlags.Russian, new LanguageModel(100)},
            {LanguageFlags.Spanish, new LanguageModel(110)},
            {LanguageFlags.Thai, new LanguageModel(120)},
            {LanguageFlags.Vietnamese, new LanguageModel(130)},
            {LanguageFlags.NA, new LanguageModel(254)},
            {LanguageFlags.Other, new LanguageModel(255)},
        };

        public Dictionary<TagNamespaceFlags, TagNamespaceModel> TagNamespace { get; } = new Dictionary<TagNamespaceFlags, TagNamespaceModel>
        {
            {TagNamespaceFlags.Reclass, new TagNamespaceModel(1)},
            {TagNamespaceFlags.Language, new TagNamespaceModel(2)},
            {TagNamespaceFlags.Parody, new TagNamespaceModel(3)},
            {TagNamespaceFlags.Character, new TagNamespaceModel(4)},
            {TagNamespaceFlags.Group, new TagNamespaceModel(5)},
            {TagNamespaceFlags.Artist, new TagNamespaceModel(6)},
            {TagNamespaceFlags.Male, new TagNamespaceModel(7)},
            {TagNamespaceFlags.Female, new TagNamespaceModel(8)},
        };

        public Dictionary<CategoryFlags,UconfigCategoryModel> Category { get; } = new Dictionary<CategoryFlags, UconfigCategoryModel>
        {
            {CategoryFlags.Misc, new UconfigCategoryModel(0x1)},
            {CategoryFlags.Doujinshi, new UconfigCategoryModel(0x2)},
            {CategoryFlags.Manga, new UconfigCategoryModel(0x4)},
            {CategoryFlags.ArtistCG, new UconfigCategoryModel(0x8)},
            {CategoryFlags.GameCG, new UconfigCategoryModel(0x10)},
            {CategoryFlags.ImageSet, new UconfigCategoryModel(0x20)},
            {CategoryFlags.Cosplay, new UconfigCategoryModel(0x40)},
            {CategoryFlags.AsianPorn, new UconfigCategoryModel(0x80)},
            {CategoryFlags.Nonh, new UconfigCategoryModel(0x100)},
            {CategoryFlags.Western, new UconfigCategoryModel(0x200)},
        };

        protected override string Separator { get; } = "-";

        [BoolValue("uh", OnValue = "y", OffValue = "n", Separator = UConfigItemSeparator)]
        public bool LoadFromHentaiAtHome { get; set; } = true;

        [IntValue("rx", Separator = UConfigItemSeparator)]
        public int ScaleWidth { get; set; } = 0;

        [IntValue("ry", Separator = UConfigItemSeparator)]
        public int ScaleHeight { get; set; } = 0;

        [EnumValue("tl", Separator = UConfigItemSeparator)]
        public GalleryTitle GalleryTitle { get; set; } = GalleryTitle.Default;

        [IntValue("ar", Separator = UConfigItemSeparator)]
        public ArchiverDownload ArchiverDownload { get; set; } = ArchiverDownload.MAMS;

        [EnumValue("dm", Separator = UConfigItemSeparator)]
        public LayoutMode LayoutMode { get; set; } = LayoutMode.List;

        [BoolValue("prn", OnValue = "y", OffValue = "n", Separator = UConfigItemSeparator)]
        public bool Popular { get; set; } = true;

        [IntValue("cats", Separator = UConfigItemSeparator)]
        public int DefaultCategories => Category.Sum(item => item.Value.Value);

        [EnumValue("fs", Separator = UConfigItemSeparator)]
        public FavoritesSort FavoritesSort { get; set; } = FavoritesSort.FavoritedTime;

        [IntValue("xns", Separator = UConfigItemSeparator)]
        public int ExcludedNamespaces => TagNamespace.Sum(item => item.Value.Value);

        [IntValue("rc", Separator = UConfigItemSeparator)]
        public ResultCount ResultCount { get; set; } = ResultCount.C25;

        [BoolValue("lt", OnValue = "m", OffValue = "p", Separator = UConfigItemSeparator)]
        public bool MouseOver { get; set; } = true;

        [EnumValue("ts", Separator = UConfigItemSeparator)]
        public PreviewSize PreviewSize { get; set; } = PreviewSize.Large;

        [IntValue("tr", Separator = UConfigItemSeparator)]
        public PreviewRowCount PreviewRowCount { get; set; } = PreviewRowCount.R4;

        [EnumValue("cs", Separator = UConfigItemSeparator)]
        public CommentsSort CommentsSort { get; set; } = CommentsSort.OldestFirst;

        [EnumValue("to", Separator = UConfigItemSeparator)]
        public TagSort TagSort { get; set; } = TagSort.Alphabetically;

        [EnumValue("ms", Separator = UConfigItemSeparator)]
        public MultiPageStyle MultiPageStyle { get; set; } = MultiPageStyle.N;

        [BoolValue("mt", OnValue = "y", OffValue = "n", Separator = UConfigItemSeparator)]
        public bool MultiPageThumb { get; set; } = false;

        [BoolValue("pn", OnValue = "1", OffValue = "0", Separator = UConfigItemSeparator)]
        public bool ShowGalleryIndex { get; set; } = false;

        [IntValue("sc", Separator = UConfigItemSeparator)]
        public CommentVotes CommentVotes { get; set; } = CommentVotes.Pop;

        [StringValue("ru", Separator = UConfigItemSeparator)]
        public string RatingColor { get; set; } = "rrggb";

        [EnumValue("xr", Separator = UConfigItemSeparator)]
        public ImageSize ImageSize { get; set; } = ImageSize.Auto;

        [BoolValue("sa", OnValue = "y", OffValue = "ohnoyoudont", Separator = UConfigItemSeparator)]
        public bool Advertisement { get; set; } = false;

        [BoolValue("oi", OnValue = "y", OffValue = "n", Separator = UConfigItemSeparator)]
        public bool AlwaysOriginal { get; set; } = false;

        [BoolValue("qb", OnValue = "y", OffValue = "n", Separator = UConfigItemSeparator)]
        public bool MultiPage { get; set; } = false;

        [BoolValue("tf", OnValue = "y", OffValue = "n", Separator = UConfigItemSeparator)]
        public bool EnableTagFlagging { get; set; } = false;

        [StringValue("hp", Separator = UConfigItemSeparator)]
        public string HentaiAtHomeClientIpPort { get; set; } = "";

        [StringValue("hk", Separator = UConfigItemSeparator)]
        public string HentaiAtHomeClientPasskey { get; set; } = "";

        [StringValue("xl", Separator = UConfigItemSeparator)]
        public string ExcludedLanguages => string.Join("", Language.Select(item => item.Value.Value)).TrimEnd('x');
    }
}