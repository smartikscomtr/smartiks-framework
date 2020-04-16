using Smartiks.Framework.Text.Markdown.Extension;
using Smartiks.Framework.Text.Markdown.Interface;

namespace Smartiks.Framework.Text.Markdown.Parser
{
    public class ItalicMarkdownParser : IMarkdownParser
    {
        public string Parse(string markdown)
        {
            return markdown.ReplaceLineByLine("(?<!_)_([^_]+)_(?!_)", match => $"<em>{ match.Groups[1] }</em>");
        }
    }
}