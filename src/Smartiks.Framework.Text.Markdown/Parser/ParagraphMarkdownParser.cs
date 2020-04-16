using Smartiks.Framework.Text.Markdown.Extension;
using Smartiks.Framework.Text.Markdown.Interface;

namespace Smartiks.Framework.Text.Markdown.Parser
{
    public class ParagraphMarkdownParser : IMarkdownParser
    {
        public string Parse(string markdown)
        {
            return markdown.ReplaceLineByLine(@"(^[^#*].*)", match => $"<p>{ match.Groups[1] }</p>");
        }
    }
}