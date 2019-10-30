using System;

namespace Smartiks.Framework.Text.Abstractions
{
    public interface ITemplateService
    {
        string Format<TContext>(string template, TContext context, Func<string, string> transform, IFormatProvider formatProvider);
    }
}