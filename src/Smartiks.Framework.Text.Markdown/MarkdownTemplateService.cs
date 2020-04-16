using Smartiks.Framework.Text.Markdown.Interface;
using Smartiks.Framework.Text.Markdown.Parser;
using System.Collections.Generic;
using System.Linq;

namespace Smartiks.Framework.Text
{
    /// <summary>
    ///  MarkdownTemplateService.Parse("__Bu şekilde çağırılarak kullanılabilir__")
    /// </summary>
    public class MarkdownTemplateService
    {
        private readonly string _markdown;
        private readonly ICollection<IMarkdownParser> _parsers;

        private MarkdownTemplateService(string markdown)
        {
            _markdown = markdown;
            _parsers = new List<IMarkdownParser>();
        }

        public MarkdownTemplateService Register(IMarkdownParser parser)
        {
            _parsers.Add(parser);

            return this;
        }

        public static string Parse(string markdown)
        {
            return new MarkdownTemplateService(markdown)
                .Register(new ParagraphMarkdownParser())
                .Register(new ItalicMarkdownParser())
                .Register(new BoldMarkdownParser())
                .Register(new HeaderMarkdownParser())
                .Register(new UnorderedListMarkdownParser())
                .ToHtml();
        }

        private string ToHtml()
        {
            return _parsers.Aggregate(_markdown, (html, parser) => parser.Parse(html));
        }
    }
}