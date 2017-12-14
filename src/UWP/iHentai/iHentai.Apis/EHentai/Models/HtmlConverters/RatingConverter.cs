using System;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using Html2Model;

namespace iHentai.Apis.EHentai.Models.HtmlConverters
{
    public class RatingConverter : IHtmlConverter
    {
        public object ReadHtml(INode node, Type targetType, object existingValue)
        {
            var match = Regex.Match((node as IElement).GetAttribute("style"),
                "background-position:-?(\\d+)px -?(\\d+)px");
            var num1 = Convert.ToInt32(match.Groups[1].Value);
            var num2 = Convert.ToInt32(match.Groups[2].Value);
            var rate = 5d;
            rate = rate - num1 / 16d;
            return num2 == 21 ? --rate + 0.5 : rate;
        }
    }
}