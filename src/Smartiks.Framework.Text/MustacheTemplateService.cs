using Mustache;
using Smartiks.Framework.Text.Abstractions;
using System;
using System.Globalization;

namespace Smartiks.Framework.Text
{
    public class MustacheTemplateService : ITemplateService
    {
        protected FormatCompiler FormatCompiler { get; }

        public MustacheTemplateService()
        {
            FormatCompiler =
                new FormatCompiler
                {
                    AreExtensionTagsAllowed = true,
                    RemoveNewLines = true
                };
        }

        public string Format<TContext>(string template, TContext context, Func<string, string> transform, IFormatProvider formatProvider)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (transform == null)
            {
                transform = s => s;
            }

            if (formatProvider == null)
            {
                formatProvider = CultureInfo.CurrentCulture;
            }

            var generator = FormatCompiler.Compile(template);

            generator.TagFormatted +=
                (sender, args) => {
                    if (args.IsExtension)
                        return;

                    args.Substitute = transform.Invoke(args.Substitute);
                };

            return generator.Render(formatProvider, context);
        }
    }
}