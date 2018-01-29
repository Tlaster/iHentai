using System;
using System.Collections.Generic;
using Html2Markdown.Replacement;

namespace Html2Markdown.Scheme
{
    /// <summary>
    ///     Collection of IReplacer for converting vanilla Markdown
    /// </summary>
    public class Markdown : IScheme
    {
        public virtual IEnumerable<IReplacer> Replacers()
        {
            yield return new PatternReplacer
            {
                Pattern = "<span class=\"ellipsis\">([^<]*)</span>",
                Replacement = @"$1..."
            };
            yield return new PatternReplacer
            {
                Pattern = "<span class=\"invisible\">[^<]*</span>|</?span[^>]*>",
                Replacement = @""
            };
            yield return new PatternReplacer
            {
                Pattern = @"</?(strong|b)>",
                Replacement = @"**"
            };
            yield return new PatternReplacer
            {
                Pattern = @"</?(em|i)>",
                Replacement = @"*"
            };
            yield return new PatternReplacer
            {
                Pattern = @"</h[1-6]>",
                Replacement = Environment.NewLine + Environment.NewLine
            };
            yield return new PatternReplacer
            {
                Pattern = @"<h1[^>]*>",
                Replacement = Environment.NewLine + Environment.NewLine + "# "
            };
            yield return new PatternReplacer
            {
                Pattern = @"<h2[^>]*>",
                Replacement = Environment.NewLine + Environment.NewLine + "## "
            };
            yield return new PatternReplacer
            {
                Pattern = @"<h3[^>]*>",
                Replacement = Environment.NewLine + Environment.NewLine + "### "
            };
            yield return new PatternReplacer
            {
                Pattern = @"<h4[^>]*>",
                Replacement = Environment.NewLine + Environment.NewLine + "#### "
            };
            yield return new PatternReplacer
            {
                Pattern = @"<h5[^>]*>",
                Replacement = Environment.NewLine + Environment.NewLine + "##### "
            };
            yield return new PatternReplacer
            {
                Pattern = @"<h6[^>]*>",
                Replacement = Environment.NewLine + Environment.NewLine + "###### "
            };
            yield return new PatternReplacer
            {
                Pattern = @"<hr[^>]*>",
                Replacement = Environment.NewLine + Environment.NewLine + "* * *" + Environment.NewLine
            };
            yield return new PatternReplacer
            {
                Pattern = @"<!DOCTYPE[^>]*>",
                Replacement = ""
            };
            yield return new PatternReplacer
            {
                Pattern = @"</?html[^>]*>",
                Replacement = ""
            };
            yield return new PatternReplacer
            {
                Pattern = @"</?head[^>]*>",
                Replacement = ""
            };
            yield return new PatternReplacer
            {
                Pattern = @"</?body[^>]*>",
                Replacement = ""
            };
            yield return new PatternReplacer
            {
                Pattern = @"<title[^>]*>.*?</title>",
                Replacement = ""
            };
            yield return new PatternReplacer
            {
                Pattern = @"<meta[^>]*>",
                Replacement = ""
            };
            yield return new PatternReplacer
            {
                Pattern = @"<link[^>]*>",
                Replacement = ""
            };
            yield return new PatternReplacer
            {
                Pattern = @"<!--[^-]+-->",
                Replacement = ""
            };
            yield return new CustomReplacer
            {
                CustomAction = HtmlParser.ReplaceImg
            };
            yield return new CustomReplacer
            {
                CustomAction = HtmlParser.ReplaceLists
            };
            yield return new CustomReplacer
            {
                CustomAction = HtmlParser.ReplaceAnchor
            };
            //new CustomReplacer
            //{
            //    CustomAction = HtmlParser.ReplaceSpan
            //};
            yield return new CustomReplacer
            {
                CustomAction = HtmlParser.ReplaceCode
            };
            yield return new CustomReplacer
            {
                CustomAction = HtmlParser.ReplacePre
            };
            yield return new CustomReplacer
            {
                CustomAction = HtmlParser.ReplaceParagraph
            };
            yield return new PatternReplacer
            {
                Pattern = @"<br[^>]*>",
                Replacement = @"  " + Environment.NewLine
            };
            yield return new CustomReplacer
            {
                CustomAction = HtmlParser.ReplaceBlockquote
            };
            yield return new CustomReplacer
            {
                CustomAction = HtmlParser.ReplaceEntites
            };
        }
    }
}