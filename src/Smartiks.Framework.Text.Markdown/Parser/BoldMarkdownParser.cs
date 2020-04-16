using Smartiks.Framework.Text.Markdown.Extension;
using Smartiks.Framework.Text.Markdown.Interface;

namespace Smartiks.Framework.Text.Markdown.Parser
{
    public class BoldMarkdownParser : IMarkdownParser
    {
        public string Parse(string markdown)
        {
            return markdown.ReplaceLineByLine("__([^_]+)__", match => $"<strong>{ match.Groups[1] }</strong>");
        }
    }
}