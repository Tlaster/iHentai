using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using iHentai.Services.Core;
using iHentai.Services.Core.Common;
using iHentai.Services.EHentai.Models;

namespace iHentai.Services.EHentai
{
    public class UConfig : AutoString, IApiConfig
    {
        public Dictionary<string, LanguageModel> Language { get; } = new Dictionary<string, LanguageModel>
        {
            {"Japanese", new LanguageModel(0)},
            {"English", new LanguageModel(1)},
            {"Chinese", new LanguageModel(10)},
            {"Dutch", new LanguageModel(20)},
            {"French", new LanguageModel(30)},
            {"German", new LanguageModel(40)},
            {"Hungarian", new LanguageModel(50)},
            {"Italian", new LanguageModel(60)},
            {"Korean", new LanguageModel(70)},
            {"Polish", new LanguageModel(80)},
            {"Portuguese", new LanguageModel(90)},
            {"Russian", new LanguageModel(100)},
            {"Spanish", new LanguageModel(110)},
            {"Thai", new LanguageModel(120)},
            {"Vietnamese", new LanguageModel(130)},
            {"N/A", new LanguageModel(254)},
            {"Other", new LanguageModel(255)},
        };

        public Dictionary<string, TagNamespaceModel> TagNamespace { get; } = new Dictionary<string, TagNamespaceModel>
        {
            {"reclass", new TagNamespaceModel(1)},
            {"language", new TagNamespaceModel(2)},
            {"parody", new TagNamespaceModel(3)},
            {"character", new TagNamespaceModel(4)},
            {"group", new TagNamespaceModel(5)},
            {"artist", new TagNamespaceModel(6)},
            {"male", new TagNamespaceModel(7)},
            {"female", new TagNamespaceModel(8)},
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
    }
}