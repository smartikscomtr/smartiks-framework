using Smartiks.Framework.Text.Markdown.Extension;
using Smartiks.Framework.Text.Markdown.Interface;

namespace Smartiks.Framework.Text.Markdown.Parser
{
    public class HeaderMarkdownParser : IMarkdownParser
    {
        public string Parse(string markdown)
        {
            return markdown.ReplaceLineByLine(@"^(#{1,6})\s?(.*)", match => string.Format("<h{0}>{1}</h{0}>", match.Groups[1].Length, match.Groups[2]));
        }
    }
}