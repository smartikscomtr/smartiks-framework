using Smartiks.Framework.Text.Markdown.Extension;
using Smartiks.Framework.Text.Markdown.Interface;
using System;

namespace Smartiks.Framework.Text.Markdown.Parser
{
    public class UnorderedListMarkdownParser : IMarkdownParser
    {
        public string Parse(string markdown)
        {
            return markdown
                .ReplaceLineByLine(@"^\*\s?(.*)", match => $"<li>{ match.Groups[1] }</li>")
                .Replace("\n", String.Empty)
                .Replace(@"(<li>.+<\/li>)", match => $"<ul>{ match.Groups[1] }</ul>");
        }
    }
}