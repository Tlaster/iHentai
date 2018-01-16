using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Windows.UI.Xaml.Data;
using Html2Markdown;
using Html2Markdown.Replacement;
using Html2Markdown.Scheme;

namespace Conet.Apis.Weibo.Views.Converters
{
    internal class WeiboContentConverter : IValueConverter
    {
        private static readonly Converter Converter = new Converter(new WeiboScheme());

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return Converter.Convert((value + "").EscapingEntities());
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    internal class WeiboScheme : IScheme
    {
        public const string AT = "@[^,\uff0c\uff1a:\\s@]+";
        public const string TOPIC = "#[^#]+#";
        public const string EMOJI = "\\[[\\w]+\\]";
        public const string URL = "http://[-a-zA-Z0-9+&@#/%?=~_|!:,.;]*[-a-zA-Z0-9+&@#/%=~_|]";
        public readonly string REGEX = $"({AT})|({TOPIC})|({EMOJI})|({URL})";

        public IList<IReplacer> Replacers()
        {
            return new List<IReplacer>
            {
                new EmojiReplacer(),
                new LongWeiboReplacer(),
                new UserNameReplacer(),
                new TopicReplacer(),
                new UrlReplacer()
            };
        }
    }

    public enum WeiboLinkType
    {
        More,
        Link,
        User,
        Topic
    }

    internal static class MarkdownStringExtensions
    {
        public static string EscapingEntities(this string text)
        {
            return Regex.Replace(text, "\\\\|\\`|\\*|_|\\{|\\}|\\(|\\)|\\+|\\-|\\!",
                match => $@"\{match.Value}");
        }
    }

    internal class EmojiReplacer : IReplacer
    {
        public string Replace(string html)
        {
            return Regex.Replace(html, WeiboScheme.EMOJI, match => true/*TODO*/ ? $"!{match.Value}(https://github.com/adam-p/markdown-here/raw/master/src/common/images/icon48.png)" : match.Value.Replace("[", "\\[").Replace("]", "\\]"));
        }
    }

    internal class LongWeiboReplacer : IReplacer
    {
        public string Replace(string html)
        {
            return html.Replace("http://m.weibo.cn/client/version", $"[全文](//{WeiboLinkType.More}:)");
        }
    }

    internal class UrlReplacer : IReplacer
    {
        public string Replace(string html)
        {
            return Regex.Replace(html, WeiboScheme.URL, match => $"[网页链接](//{WeiboLinkType.Link}:{match.Value})");
        }
    }

    internal class UserNameReplacer : IReplacer
    {
        public string Replace(string html)
        {
            return Regex.Replace(html, WeiboScheme.AT,
                match => $"[{match.Value}](//{WeiboLinkType.User}:{match.Value})");
        }
    }

    internal class TopicReplacer : IReplacer
    {
        public string Replace(string html)
        {
            return Regex.Replace(html, WeiboScheme.TOPIC,
                match => $"[{match.Value}](//{WeiboLinkType.Topic}:{match.Value})");
        }
    }
}