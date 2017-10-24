using System;
using AngleSharp.Dom;
using AngleSharp.Extensions;
using Html2Model;

namespace iHentai.Services.EHentai.Models
{
    public enum CategoryFlags
    {
//        [StringValue("MISC")]
//        [HexColor("607D8B")]
//        [SearchOption("f_misc")]
        Misc,

//        [StringValue("DOUJINSHI")]
//        [HexColor("F44336")]
//        [SearchOption("f_doujinshi")]
        Doujinshi,

//        [StringValue("MANGA")]
//        [HexColor("FFC107")]
//        [SearchOption("f_manga")]
        Manga,

//        [StringValue("ARTIST CG")]
//        [HexColor("FFEB3B")]
//        [SearchOption("f_artistcg")]
        ArtistCG,

//        [StringValue("GAME CG")]
//        [HexColor("4CAF50")]
//        [SearchOption("f_gamecg")]
        GameCG,

//        [StringValue("IMAGE SET")]
//        [HexColor("3F51B5")]
//        [SearchOption("f_imageset")]
        ImageSet,

//        [StringValue("COSPLAY")]
//        [HexColor("9C27B0")]
//        [SearchOption("f_cosplay")]
        Cosplay,

//        [StringValue("ASIAN PORN")]
//        [HexColor("E91E63")]
//        [SearchOption("f_asianporn")]
        AsianPorn,

//        [StringValue("NON-H")]
//        [HexColor("2196F3")]
//        [SearchOption("f_non-h")]
        Nonh,

//        [StringValue("WESTERN")]
//        [HexColor("8BC34A")]
//        [SearchOption("f_western")]
        Western
    }

    public class CategoryConverter : IHtmlConverter
    {
        public object ReadHtml(INode node, Type targetType, object existingValue)
        {
            var text = (node as IElement).GetAttribute("alt").ToLowerInvariant();
            switch (text)
            {
                case "non-h":
                    return CategoryFlags.Nonh;
                default:
                    return (CategoryFlags)Enum.Parse(typeof(CategoryFlags), text, true);
            }
        }
    }
}