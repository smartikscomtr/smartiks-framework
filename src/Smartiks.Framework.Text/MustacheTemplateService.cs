using Mustache;
using System;
using Smartiks.Framework.Text.Abstractions;

namespace Smartiks.Framework.Text
{
    public class MustacheTemplateService : ITemplateService
    {
        protected FormatCompiler FormatCompiler { get; set; } = new FormatCompiler();

        public MustacheTemplateService()
        {
        }

        public string Format<TContext>(string template, TContext context, Func<String, String> escaper, IFormatProvider formatProvider)
        {
            FormatCompiler.AreExtensionTagsAllowed = true;

            FormatCompiler.RemoveNewLines = true;


            var generator = FormatCompiler.Compile(template);

            generator.TagFormatted +=
                delegate(object sender, TagFormattedEventArgs args) {

                    if (args.IsExtension)
                        return;

                    args.Substitute = escaper(args.Substitute);
                };

            return generator.Render(formatProvider, context);
        }
    }
}
